using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Verifications.Dtos;
using MediatR;

namespace CandidateManagement.Application.Verifications.Queries;

public class GetVerificationResultQueryHandler : IRequestHandler<GetVerificationResultQuery, VerificationResultDto>
{
    private readonly IVerificationRepository _verificationRepository;

    public GetVerificationResultQueryHandler(IVerificationRepository verificationRepository)
    {
        _verificationRepository = verificationRepository;
    }

    public async Task<VerificationResultDto> Handle(GetVerificationResultQuery request, CancellationToken cancellationToken)
    {
        var verification = await _verificationRepository.GetByIdAsync(request.VerificationId) ?? throw new VerificationDomainException("Verification not found");
        return new VerificationResultDto
        {
            VerificationId = verification.Id,
            SearchedFullName = verification.SearchedFullName,
            Status = verification.Status.ToString(),
            FoundCandidateIds = verification.FoundCandidateIds.ToList(),
            FoundEmployeeIds = verification.FoundEmployeeIds.ToList(),
            StartedAt = verification.StartedAt,
            CompletedAt = verification.CompletedAt
        };
    }
}