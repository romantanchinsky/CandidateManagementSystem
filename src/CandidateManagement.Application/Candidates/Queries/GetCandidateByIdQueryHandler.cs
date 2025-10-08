using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Interfaces;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Candidates.Queries;

public class GetCandidateByIdQueryHandler : IRequestHandler<GetCandidateByIdQuery, CandidateReadDto>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateByIdQueryHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }
    public async Task<CandidateReadDto> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id) ?? throw new CandidateDomainException("Candidate not found");
        return candidate.Adapt<CandidateReadDto>();
    }
}