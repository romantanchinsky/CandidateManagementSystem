using CandidateManagement.Application.Auth.Dtos;
using MediatR;

namespace CandidateManagement.Application.Auth.Commands;

public sealed record LoginCommand(
    string Login,
    string Password) : IRequest<TokensDto>;