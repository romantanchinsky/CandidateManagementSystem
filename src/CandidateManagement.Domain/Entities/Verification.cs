using CandidateManagement.Domain.Interfaces;

namespace CandidateManagement.Domain.Entities
{
    public class Verification : IAggregateRoot
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        private List<Guid> _foundCandidateIds = new();
        private List<Guid> _foundEmployeeIds = new();

        public string SearchedFullName { get; private set; }
        public VerificationStatus Status { get; private set; }
        public IReadOnlyCollection<Guid> FoundCandidateIds => _foundCandidateIds.AsReadOnly();
        public IReadOnlyCollection<Guid> FoundEmployeeIds => _foundEmployeeIds.AsReadOnly();
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public Guid WhoStartedId { get; private set; }

        private Verification()
        {
        }

        public static Verification StartVerification(string fullName, Guid whoStartedId)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new VerificationDomainException("Full name is required");

            return new Verification
            {
                Id = Guid.NewGuid(),
                SearchedFullName = fullName.Trim(),
                Status = VerificationStatus.InProgress,
                StartedAt = DateTime.UtcNow,
                WhoStartedId = whoStartedId
            };
        }

        public void AddFoundCandidates(List<Guid> candidateIds)
        {
            if (Status != VerificationStatus.InProgress)
                throw new VerificationDomainException("Verification is not in progress");

            _foundCandidateIds.AddRange(candidateIds.Where(id => !_foundCandidateIds.Contains(id)));
        }

        public void AddFoundEmployees(List<Guid> employeeIds)
        {
            if (Status != VerificationStatus.InProgress)
                throw new VerificationDomainException("Verification is not in progress");

            _foundEmployeeIds.AddRange(employeeIds.Where(id => !_foundEmployeeIds.Contains(id)));
        }

        public void CompleteVerification()
        {
            if (Status != VerificationStatus.InProgress)
                throw new VerificationDomainException("Verification is not in progress");

            Status = VerificationStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            Status = VerificationStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public enum VerificationStatus
    {
        InProgress = 0,
        Completed = 1,
        Failed = 2
    }
}