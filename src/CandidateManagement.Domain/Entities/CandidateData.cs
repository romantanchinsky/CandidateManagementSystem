using CandidateManagement.Domain.ValueObjects;

namespace CandidateManagement.Domain.Entities;

public class CandidateData
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public string Country { get; private set; } = null!;
    public DateTime DateOfBirth { get; private set; }

    private readonly List<SocialNetwork> _socialNetworks = new List<SocialNetwork>();
    public IReadOnlyCollection<SocialNetwork> SocialNetworks => _socialNetworks;

    private CandidateData() { }

    public CandidateData(
        FullName fullName,
        Email email,
        PhoneNumber phoneNumber,
        string country,
        DateTime dateofBirth)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Country = country;
        DateOfBirth = dateofBirth;
    }

    public void AddSocialNetwork(string username, string type)
    {
        _socialNetworks.Add(new SocialNetwork(username, type));
    }
}