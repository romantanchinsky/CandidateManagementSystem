using CandidateManagement.Application.Candidates.Dtos;
using MediatR;

namespace CandidateManagement.Application.Candidates.Queries;

public record GetCandidatesWithPaginationQuery : IRequest<PaginatedList<CandidateReadDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchQuery { get; init; }
    public string? WorkScheduleFilter { get; init; } // "Office,Hybrid,Remote"
    public bool OnlyMine { get; init; } = false;
    public Guid CurrentUserId { get; init; }
}