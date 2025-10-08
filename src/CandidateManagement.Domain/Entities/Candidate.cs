using CandidateManagement.Domain.Enums;
using CandidateManagement.Domain.Interfaces;

namespace CandidateManagement.Domain.Entities;

public class Candidate : IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public CandidateData CandidateData { get; private set; }
    public WorkSchedule WorkSchedule { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public Guid WorkingGroupId { get; private set; }
    public Guid CreatedByUserId { get; private set; }

    private Candidate() { }

    public Candidate(
        CandidateData candidateData,
        WorkSchedule workSchedule,
        Guid workingGroupId,
        Guid createByUserId)
    {
        CandidateData = candidateData ?? throw new ArgumentNullException(nameof(candidateData));
        WorkSchedule = workSchedule;
        WorkingGroupId = workingGroupId;
        CreatedByUserId = createByUserId;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateWorkSchedule(WorkSchedule workSchedule)
    {
        WorkSchedule = workSchedule;
        LastUpdated = DateTime.UtcNow;
    }
    public void UpdateData(CandidateData newCandidateData)
    {
        CandidateData = newCandidateData ?? throw new ArgumentNullException(nameof(newCandidateData));
        LastUpdated = DateTime.UtcNow;
    }

    public void AssignToWorkingGroup(Guid newWorkingGroupId)
    {
        if (newWorkingGroupId == Guid.Empty) throw new ArgumentException("group id invalid", nameof(newWorkingGroupId));
        WorkingGroupId = newWorkingGroupId;
        LastUpdated = DateTime.UtcNow;
    }

    public void ChangeCreatedByUser(Guid newHrUserId)
    {
        if (newHrUserId == Guid.Empty)
            throw new CandidateDomainException("HR user ID cannot be empty");

        CreatedByUserId = newHrUserId;
        LastUpdated = DateTime.UtcNow;
    }
}