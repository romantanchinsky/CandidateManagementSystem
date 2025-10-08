using System.ComponentModel.DataAnnotations;

namespace CandidateManagement.Application.Auth.Requests;

public sealed record LogoutRequest(
    [Required]
    string RefreshToken
);