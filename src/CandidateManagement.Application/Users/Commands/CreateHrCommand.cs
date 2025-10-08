using CandidateManagement.Application.Users.Dtos;
using CandidateManagement.Domain.ValueObjects;
using MediatR;

namespace CandidateManagement.Application.Users.Commands;

public sealed record CreateHrCommand(
    FullName FullName,
    string Login,
    string Password,
    Guid CurrentUserId
) : IRequest<UserReadDto>;