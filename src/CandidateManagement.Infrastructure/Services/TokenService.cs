using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CandidateManagement.Application.Auth.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Services;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CandidateManagement.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _tokenRepository;
    public TokenService(IConfiguration configuration, ITokenRepository tokenRepository)
    {
        _configuration = configuration;
        _tokenRepository = tokenRepository;
    }

    private string GenerateAccessToken(TokenClaims tokenClaims)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Token:JWT:Secret"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new[]
        {
            new Claim("userId", tokenClaims.UserId.ToString()),
            new Claim("userRole", tokenClaims.UserRole.ToString()),
            new Claim("workingGroupId", tokenClaims.WorkingGroupId?.ToString() ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Token:Jwt:ExpiryMinutes"])),
            SigningCredentials = credentials,
            Issuer = _configuration["Token:Jwt:Issuer"],
            Audience = _configuration["Token:Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<TokensDto> CreateTokensAsync(TokenClaims tokenClaims)
    {
        var accessToken = GenerateAccessToken(tokenClaims);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken(
            refreshToken,
            tokenClaims.UserId,
            DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Token:Rt:ExpiryDays"]))
        );
        await _tokenRepository.AddAsync(refreshTokenEntity);

        return new TokensDto(accessToken, refreshToken);
    }

    public async Task<TokensDto> RefreshTokensAsync(TokensDto tokensDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokensDto.AccessToken);
        if (!Guid.TryParse(principal.FindFirst("userId")?.Value, out Guid userId))
        {
            throw new SecurityTokenException("Invalid token");
        }
        if (!Enum.TryParse(principal.FindFirst("userRole")?.Value, out Role role))
        {
            throw new SecurityTokenException("Invalid token");
        }
        var workingGroupIdClaim = principal.FindFirst("workingGroupId")?.Value;

        var storedRefreshToken = await _tokenRepository.GetByValueAsync(tokensDto.RefreshToken);
        if (storedRefreshToken == null
            || storedRefreshToken.UserId != userId
            || storedRefreshToken.ExpiresAt < DateTime.UtcNow
            || storedRefreshToken.IsRevoked)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }
        await _tokenRepository.RevokeTokenByValueAsync(storedRefreshToken.Token);

        var newTokens = await CreateTokensAsync(new TokenClaims(userId, role, string.IsNullOrEmpty(workingGroupIdClaim) ? null : Guid.Parse(workingGroupIdClaim)));
        return newTokens;
    }

    public async Task RevokeRefreshToken(string refreshToken)
    {

        await _tokenRepository.RevokeTokenByValueAsync(refreshToken);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Token:Jwt:Secret"]!)),
            ValidateLifetime = false,
            ValidIssuer = _configuration["Token:Jwt:Issuer"],
            ValidAudience = _configuration["Token:Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        return principal;
    }
}