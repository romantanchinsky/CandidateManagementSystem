namespace CandidateManagement.Domain.Entities;

public class SocialNetwork
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Username { get; private set; }
    public string Type { get; private set; }
    public DateTime DateAdded { get; private set; } = DateTime.UtcNow;

    private SocialNetwork() { }
    public SocialNetwork(string username, string type)
    {
        Username = username;
        Type = type;
    }
} 