using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Commands;
public record UpdateWorkingGroupMembersCommand : IRequest<Unit>
{
    public Guid WorkingGroupId { get; init; }
    public List<Guid> UserIds { get; init; } = new();
    public Guid CurrentUserId { get; init; }
}