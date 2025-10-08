using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface IWorkingGroupRepository
{
    Task AddAsync(WorkingGroup workingGroup);
    Task<WorkingGroup?> GetByNameAsync(string name);
    Task<WorkingGroup?> GetByIdAsync(Guid id);
}