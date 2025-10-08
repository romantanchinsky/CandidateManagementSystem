using CandidateManagement.Application.Users.Commands;
using CandidateManagement.Application.Users.Dtos;
using CandidateManagement.Application.Users.Queries;
using CandidateManagement.Application.Users.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("[controller]")]
[SwaggerTag("Управление пользователями - создание HR, получение информации о пользователях")]
public class UserController : CustomControllerBase
{
    private readonly ISender _mediatr;

    public UserController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [SwaggerOperation(
        Summary = "Получить пользователя по ID",
        Description = "Возвращает информацию о пользователе по его идентификатору"
    )]
    [SwaggerResponse(200, "Пользователь найден", typeof(UserReadDto))]
    [SwaggerResponse(404, "Пользователь не найден")]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    [HttpGet("{id}", Name = "GetUserByIdAsync")]
    public async Task<ActionResult<UserReadDto>> GetUserByIdAsync(Guid id)
    {
        var query = new GetuserByIdQuery(id);
        var result = await _mediatr.Send(query);
        return Ok(result);

    }

    [HttpPost("hr")]
    [SwaggerOperation(
        Summary = "Создать HR пользователя",
        Description = "Создание нового пользователя с ролью HR менеджера"
    )]
    [SwaggerResponse(201, "HR пользователь успешно создан", typeof(UserReadDto))]
    [SwaggerResponse(400, "Ошибка валидации данных")]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    public async Task<ActionResult<UserReadDto>> CreateHrAsync(CreateHrRequest createHrRequest)
    {
        var command = createHrRequest.Adapt<CreateHrCommand>() with { CurrentUserId = GetCurrentUserId() };

        var result = await _mediatr.Send(command);
        return CreatedAtRoute(nameof(GetUserByIdAsync),
            new { id = result.Id }, result);
    }

    [HttpGet("hr")]
    [SwaggerOperation(
        Summary = "Получить всех HR пользователей",
        Description = "Возвращает список всех пользователей с ролью HR"
    )]
    [SwaggerResponse(200, "Список HR пользователей получен", typeof(List<UserReadDto>))]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    public async Task<ActionResult<List<UserReadDto>>> GetAllHrs()
    {
        var query = new GetAllHrsQuery();
        var result = await _mediatr.Send(query);
        return Ok(result);
    }
}