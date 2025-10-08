using MediatR;

namespace CandidateManagement.Application.Verifications.Commands;

public record StartVerificationCommand
(
    string FullName,
    Guid CurrentUserId
) : IRequest<Guid>;