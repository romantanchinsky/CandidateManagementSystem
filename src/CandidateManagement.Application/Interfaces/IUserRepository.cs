using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByLoginAsync(string login);
    Task SaveChangesAsync();
    Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds);
    Task<User?> GetOtherHRInWorkGroupAsync(Guid workingGroupId, Guid hrUserId);
    Task<User> GetAdminUserAsync();
    Task<bool> IsAdmin(Guid id);
    Task<IReadOnlyList<User>> GetAllHrsAsync();
}