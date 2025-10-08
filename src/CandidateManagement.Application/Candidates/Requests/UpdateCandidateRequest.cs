using System.ComponentModel.DataAnnotations;
using CandidateManagement.Application.DTOs;

namespace CandidateManagement.Application.Candidates.Requests
{
    public record UpdateCandidateRequest
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; init; }

        [Required]
        [StringLength(50)]
        public string LastName { get; init; }

        [StringLength(50)]
        public string? Patronymic { get; init; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; init; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(100)]
        public string Country { get; init; }

        [Required]
        public DateTime DateOfBirth { get; init; }

        [Required]
        [RegularExpression("Office|Hybrid|Remote", ErrorMessage = "Work schedule must be Office, Hybrid, or Remote")]
        public string WorkSchedule { get; init; }

        public List<SocialNetworkCreateDto> SocialNetworks { get; init; } = new();
    }
}