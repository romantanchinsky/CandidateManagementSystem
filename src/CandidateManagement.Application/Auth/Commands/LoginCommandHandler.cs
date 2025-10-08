using System.Security.Authentication;
using CandidateManagement.Application.Auth.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Services;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokensDto>
{
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    public async Task<TokensDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(request.Login.ToLower()) ?? throw new AuthenticationException("Login not found");
        if (!_passwordHasher.VerifyHashedPassword(request.Password, user.PasswordHash))
        {
            throw new AuthenticationException("Incorrect Password");
        }
        return await _tokenService.CreateTokensAsync(user.Adapt<TokenClaims>());
    }
}