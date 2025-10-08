using CandidateManagement.Domain.Interfaces;

namespace CandidateManagement.Domain.Entities;

public class RefreshToken : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; } = false;

    private RefreshToken() { }

    public RefreshToken(
        string token,
        Guid userId,
        DateTime expiresAt)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}