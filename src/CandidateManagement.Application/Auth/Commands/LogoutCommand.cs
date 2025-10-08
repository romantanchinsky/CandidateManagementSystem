using MediatR;

namespace CandidateManagement.Application.Auth.Commands;

public sealed record LogoutCommand(
    string RefreshToken
) : IRequest;