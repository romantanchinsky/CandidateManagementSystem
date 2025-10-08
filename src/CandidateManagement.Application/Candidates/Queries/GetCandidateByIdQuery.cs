using CandidateManagement.Application.Candidates.Dtos;
using MediatR;

namespace CandidateManagement.Application.Candidates.Queries;

public sealed record GetCandidateByIdQuery(
    Guid Id
) : IRequest<CandidateReadDto>;