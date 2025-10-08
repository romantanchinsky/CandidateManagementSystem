using System.ComponentModel.DataAnnotations;

public record StartVerificationRequest
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string FullName { get; init; }
}