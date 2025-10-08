using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.Users.Requests;

public sealed record CreateHrRequest(
    [Required]
    [StringLength(50)]
    string FirstName,
    [Required]
    [StringLength(50)]
    string LastName,
    [StringLength(50)]
    string? Patronymic,
    [Required]
    [StringLength(50)]
    string Login,
    [Required]
    [StringLength(50)]
    string Password
);