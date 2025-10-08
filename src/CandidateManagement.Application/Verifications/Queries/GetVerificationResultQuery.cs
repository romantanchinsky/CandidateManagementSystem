using CandidateManagement.Application.Verifications.Dtos;
using MediatR;

namespace CandidateManagement.Application.Verifications.Queries;

public record GetVerificationResultQuery(
    Guid VerificationId
) : IRequest<VerificationResultDto>;