using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.WorkingGroups.Requests;

public sealed record CreateWorkingGroupRequest(
    [Required]
    [StringLength(100)]
    string Name
);