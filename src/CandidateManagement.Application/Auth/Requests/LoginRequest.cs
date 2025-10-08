using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.Auth.Requests;

public sealed record LoginRequest(
    [Required]
    [StringLength(50)]
    string Login,
    [Required]
    [StringLength(50)]
    string Password
);