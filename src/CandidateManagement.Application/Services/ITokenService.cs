using CandidateManagement.Application.Auth.Dtos;

namespace CandidateManagement.Application.Services;

public interface ITokenService
{
    Task<TokensDto> CreateTokensAsync(TokenClaims createTokenDto);
    Task<TokensDto> RefreshTokensAsync(TokensDto tokensDto);
    Task RevokeRefreshToken(string refreshToken);
}