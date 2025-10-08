using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.WorkingGroups.Requests;

public record UpdateWorkingGroupParticipantsRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one user ID is required")]
    public List<Guid> UserIds { get; init; } = new();
}