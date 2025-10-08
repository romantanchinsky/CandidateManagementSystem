using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly ApplicationDbContext _context;

    public CandidateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Candidate candidate)
    {
        await _context.Candidates.AddAsync(candidate);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Candidate>> GetByCreatorAsync(Guid creatorId)
    {
        return await _context.Candidates.Where(c => c.CreatedByUserId == creatorId).ToListAsync();
    }

    public async Task<Candidate?> GetByIdAsync(Guid id)
    {
        return await _context.Candidates
            .Include(c => c.CandidateData.SocialNetworks)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Candidate candidate)
    {
        _context.Entry(candidate).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Candidate>> GetCandidatesWithFiltersAsync(
        Guid workingGroupId,
        string? searchQuery = null,
        string? workScheduleFilter = null,
        bool onlyMine = false,
        Guid? currentUserId = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Candidates
            .Include(c => c.CandidateData.SocialNetworks)
            .Where(c => c.WorkingGroupId == workingGroupId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.CandidateData.FullName.FirstName, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.FullName.LastName, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.FullName.Patronymic, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.Email.Value, $"%{searchQuery}%") ||
                c.CandidateData.SocialNetworks.Any(sn => EF.Functions.ILike(sn.Username, $"%{searchQuery}%"))
            );
        }

        if (!string.IsNullOrWhiteSpace(workScheduleFilter))
        {
            var schedules = workScheduleFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(c => schedules.Contains(c.WorkSchedule.ToString()));
        }

        if (onlyMine && currentUserId.HasValue)
        {
            query = query.Where(c => c.CreatedByUserId == currentUserId.Value);
        }

        query = query.OrderByDescending(c => c.LastUpdated);

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCandidatesCountAsync(
        Guid workGroupId,
        string? searchQuery = null,
        string? workScheduleFilter = null,
        bool onlyMine = false,
        Guid? currentUserId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Candidates
            .Where(c => c.WorkingGroupId == workGroupId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.CandidateData.FullName.FirstName, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.FullName.LastName, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.FullName.Patronymic, $"%{searchQuery}%") ||
                EF.Functions.ILike(c.CandidateData.Email.Value, $"%{searchQuery}%") ||
                c.CandidateData.SocialNetworks.Any(sn => EF.Functions.ILike(sn.Username, $"%{searchQuery}%"))
            );
        }

        if (!string.IsNullOrWhiteSpace(workScheduleFilter))
        {
            var schedules = workScheduleFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(c => schedules.Contains(c.WorkSchedule.ToString()));
        }

        if (onlyMine && currentUserId.HasValue)
        {
            query = query.Where(c => c.CreatedByUserId == currentUserId.Value);
        }

        return await query
            .CountAsync(cancellationToken);
    }

    public async Task Delete(Candidate candidate)
    {
        _context.Remove(candidate);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Candidate>> SearchByNameAsync(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return new List<Candidate>();

        var searchTerms = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(term => term.ToLower())
                                  .ToList();

        var query = _context.Candidates.AsQueryable();

        foreach (var term in searchTerms)
        {
            query = query.Where(c =>
                c.CandidateData.FullName.FirstName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                c.CandidateData.FullName.LastName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                (c.CandidateData.FullName.Patronymic != null && c.CandidateData.FullName.Patronymic.Contains(term, StringComparison.CurrentCultureIgnoreCase)));
        }

        return await query.ToListAsync();
    }
}