using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface ICandidateRepository
{
    Task AddAsync(Candidate candidate);
    Task<Candidate?> GetByIdAsync(Guid id);
    Task<List<Candidate>> GetByCreatorAsync(Guid creatorId);
    Task SaveChangesAsync();
    Task UpdateAsync(Candidate candidate);
    Task<IReadOnlyList<Candidate>> GetCandidatesWithFiltersAsync(
            Guid workGroupId,
            string? searchQuery = null,
            string? workScheduleFilter = null,
            bool onlyMine = false,
            Guid? currentUserId = null,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default);
    Task<int> GetCandidatesCountAsync(
        Guid workGroupId,
        string? searchQuery = null,
        string? workScheduleFilter = null,
        bool onlyMine = false,
        Guid? currentUserId = null,
        CancellationToken cancellationToken = default
    );
    Task Delete(Candidate candidate);

    Task<List<Candidate>> SearchByNameAsync(string fullName);
}