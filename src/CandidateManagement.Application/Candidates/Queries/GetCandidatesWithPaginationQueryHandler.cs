using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Interfaces;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Candidates.Queries;

public sealed class GetCandidatesWithPaginationQueryHandler 
    : IRequestHandler<GetCandidatesWithPaginationQuery, PaginatedList<CandidateReadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public GetCandidatesWithPaginationQueryHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<PaginatedList<CandidateReadDto>> Handle(
        GetCandidatesWithPaginationQuery request, 
        CancellationToken cancellationToken)
    {
        var currentUser = await _userRepository.GetByIdAsync(request.CurrentUserId);
        if (currentUser == null)
            throw new NotFoundDomainException($"User with id: {request.CurrentUserId} not found");

        Guid workGroupId;
        if (currentUser.IsAdmin())
        {
            workGroupId = Guid.Empty;
        }
        else if (currentUser.IsHR() && currentUser.WorkingGroupId.HasValue)
        {
            workGroupId = currentUser.WorkingGroupId.Value;
        }
        else
        {
            throw new AccessDeniedDomainException("Access denied");
        }

        var candidates = await _unitOfWork.CandidateRepository.GetCandidatesWithFiltersAsync(
            workGroupId: workGroupId,
            searchQuery: request.SearchQuery,
            workScheduleFilter: request.WorkScheduleFilter,
            onlyMine: request.OnlyMine,
            currentUserId: request.OnlyMine ? request.CurrentUserId : null,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var totalCount = await _unitOfWork.CandidateRepository.GetCandidatesCountAsync(
            workGroupId: workGroupId,
            searchQuery: request.SearchQuery,
            workScheduleFilter: request.WorkScheduleFilter,
            onlyMine: request.OnlyMine,
            currentUserId: request.OnlyMine ? request.CurrentUserId : null,
            cancellationToken: cancellationToken);

        var candidateDtos = candidates.Adapt<List<CandidateReadDto>>();

        return new PaginatedList<CandidateReadDto>(candidateDtos, totalCount, request.PageNumber, request.PageSize);
    }
}