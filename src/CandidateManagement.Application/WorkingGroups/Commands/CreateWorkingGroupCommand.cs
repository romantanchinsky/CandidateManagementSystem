using CandidateManagement.Domain.Entities;
using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Commands;

public sealed record CreateWorkingGroupCommand(
    string Name,
    Guid CurrentUserId
) : IRequest<WorkingGroup>;