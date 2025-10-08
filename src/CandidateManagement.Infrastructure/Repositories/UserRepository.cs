using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        _context.SaveChanges();
    }
    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login.ToLower());
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds)
    {
        if (userIds == null || !userIds.Any())
            return new List<User>();

        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<User?> GetOtherHRInWorkGroupAsync(Guid workingGroupId, Guid hrUserId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.WorkingGroupId == workingGroupId && u.Role == Role.HR && u.Id != hrUserId);
    }
    public async Task<User> GetAdminUserAsync()
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Role == Role.Admin);
    }

    public async Task<bool> IsAdmin(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user?.IsAdmin() ?? false;
    }
    public async Task<IReadOnlyList<User>> GetAllHrsAsync()
    {
        return await _context.Users.Where(u => u.Role == Role.HR).ToListAsync();
    }
}