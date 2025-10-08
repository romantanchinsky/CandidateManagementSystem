namespace CandidateManagement.Application.Auth.Responses;

public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken
);