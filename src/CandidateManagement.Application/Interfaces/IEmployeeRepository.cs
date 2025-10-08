using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<List<Employee>> SearchByNameAsync(string fullName);
    Task SaveChangesAsync();
}