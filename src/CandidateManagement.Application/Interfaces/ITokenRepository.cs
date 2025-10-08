using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface ITokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByValueAsync(string token);
    Task RevokeTokenByValueAsync(string token);
}