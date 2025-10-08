namespace CandidateManagement.Application.Services;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyHashedPassword(string password, string passwordHash);
}