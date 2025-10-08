using CandidateManagement.Application.Auth.Commands;
using CandidateManagement.Application.Auth.Requests;
using CandidateManagement.Application.Auth.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Route("[controller]")]
[SwaggerTag("Аутентификация и управление токенами")]
public class AuthController : CustomControllerBase
{
    private readonly ISender _mediatr;
    public AuthController(ISender sender)
    {
        _mediatr = sender;
    }
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Вход в систему",
        Description = "Аутентификация пользователя и получение JWT токенов"
    )]
    [SwaggerResponse(200, "Успешная аутентификация", typeof(TokenResponse))]
    [SwaggerResponse(401, "Неверные учетные данные")]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest loginRequest)
    {
        var command = loginRequest.Adapt<LoginCommand>();
        var result = await _mediatr.Send(command);
        return Ok(result.Adapt<TokenResponse>());

    }

    [HttpPost("refresh")]
    [SwaggerOperation(
        Summary = "Обновление токенов",
        Description = "Обновление access token с использованием refresh token"
    )]
    [SwaggerResponse(200, "Токены успешно обновлены", typeof(TokenResponse))]
    [SwaggerResponse(401, "Refresh token недействителен")]
    public async Task<ActionResult<TokenResponse>> Refresh(RefreshRequest refreshRequest)
    {
        var command = refreshRequest.Adapt<RefreshTokensCommand>();
        var result = await _mediatr.Send(command);
        return Ok(result.Adapt<TokenResponse>());

    }

    [HttpPost("logout")]
     [SwaggerOperation(
        Summary = "Выход из системы",
        Description = "Завершение сессии пользователя и инвалидация токенов"
    )]
    [SwaggerResponse(204, "Выход выполнен успешно")]
    public async Task<ActionResult> Logout(LogoutRequest logoutRequest)
    {
        var command = logoutRequest.Adapt<LogoutCommand>();
        await _mediatr.Send(command);
        return NoContent();
    }
}