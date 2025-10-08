using CandidateManagement.Domain.Interfaces;

namespace CandidateManagement.Domain.Entities;

public class WorkingGroup : IAggregateRoot
{
    private readonly List<User> _participants = new();
    private readonly List<Candidate> _candidates = new();

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    public IReadOnlyCollection<User> Participants => _participants;
    public IReadOnlyCollection<Candidate> Candidates => _candidates;

    private WorkingGroup() { }

    public WorkingGroup(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name required", nameof(name));
        Name = name;
    }

    public void AddParticipant(User hr)
    {
        if (!_participants.Contains(hr)) _participants.Add(hr);
    }

    public void RemoveParticipant(User hr)
    {
        _participants.Remove(hr);
    }

    public void AddCandidate(Candidate candidate)
    {
        if (!_candidates.Contains(candidate)) _candidates.Add(candidate);
    }

    public void RemoveCandidate(Candidate candidate)
    {
        _candidates.Remove(candidate);
    }
}