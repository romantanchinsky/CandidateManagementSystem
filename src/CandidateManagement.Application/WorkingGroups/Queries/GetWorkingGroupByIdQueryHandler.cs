using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.WorkingGroups.Dtos;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Queries;

public class GetWorkingGroupByIdQueryHandler : IRequestHandler<GetWorkingGroupByIdQuery, WorkingGroupReadDto>
{
    private readonly IWorkingGroupRepository _repository;

    public GetWorkingGroupByIdQueryHandler(IWorkingGroupRepository repository)
    {
        _repository = repository;
    }
    public async Task<WorkingGroupReadDto> Handle(GetWorkingGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var workingGroup = await _repository.GetByIdAsync(request.Id) ?? throw new NotFoundDomainException($"Working group with id= {request.Id} not found");
        return workingGroup.Adapt<WorkingGroupReadDto>();
    }
}