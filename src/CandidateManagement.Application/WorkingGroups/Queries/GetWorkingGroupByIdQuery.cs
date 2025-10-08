using CandidateManagement.Application.WorkingGroups.Dtos;
using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Queries;

public sealed record GetWorkingGroupByIdQuery(
    Guid Id
) : IRequest<WorkingGroupReadDto>;