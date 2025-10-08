using CandidateManagement.Application.Auth.Dtos;
using MediatR;

namespace CandidateManagement.Application.Auth.Commands;

public sealed record RefreshTokensCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<TokensDto>;