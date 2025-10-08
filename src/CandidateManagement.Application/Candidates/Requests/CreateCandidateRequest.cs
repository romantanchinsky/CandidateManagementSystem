using System.ComponentModel.DataAnnotations;
using CandidateManagement.Application.DTOs;

namespace CandidateManagement.Application.Candidates.Requests;

public sealed record CreateCandidateRequest(
    [Required]
    [StringLength(50)]
    string FirstName,

    [Required]
    [StringLength(50)]
    string LastName,

    [StringLength(50)]
    string? Patronymic,

    [Required]
    [EmailAddress]
    [StringLength(50)]
    string Email,

    [Required]
    [Phone]
    string PhoneNumber,

    [Required]
    [StringLength(100)]
    string Country,

    [Required]
    DateTime DateOfBirth,

    List<SocialNetworkCreateDto> SocialNetworks,

    [Required]
    [RegularExpression("Office|Hybrid|Remote", ErrorMessage = "Work schedule must be Office, Hybrid, or Remote")]
    string WorkSchedule
);