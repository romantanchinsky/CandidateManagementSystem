namespace CandidateManagement.Application.Auth.Dtos;

public sealed record TokensDto(
    string AccessToken,
    string RefreshToken
);