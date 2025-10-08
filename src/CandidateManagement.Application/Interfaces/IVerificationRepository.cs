using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface IVerificationRepository
{
    Task AddAsync(Verification verification);
    Task<Verification?> GetByIdAsync(Guid id);
    
}
