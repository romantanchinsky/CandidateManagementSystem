using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.DTOs;

public record SocialNetworkCreateDto
{
    [Required]
    [StringLength(100)]
    public string Username { get; init; }

    [Required]
    [StringLength(50)]
    public string Type { get; init; }
}