using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.Auth.Requests;

public sealed record RefreshRequest(
    [Required]
    string AccessToken,
    [Required]
    string RefreshToken
);