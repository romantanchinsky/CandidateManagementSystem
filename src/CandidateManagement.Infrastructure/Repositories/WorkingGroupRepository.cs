using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories;

public class WorkingGroupRepository : IWorkingGroupRepository
{
    private readonly ApplicationDbContext _context;

    public WorkingGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(WorkingGroup workingGroup)
    {
        await _context.WorkingGroups.AddAsync(workingGroup);
        _context.SaveChanges();
    }

    public async Task<WorkingGroup?> GetByIdAsync(Guid id)
    {
        return await _context.WorkingGroups
            .Include(g => g.Participants)
            .Include(g => g.Candidates)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<WorkingGroup?> GetByNameAsync(string name)
    {
        return await _context.WorkingGroups.FirstOrDefaultAsync(g => g.Name == name);
    }
}