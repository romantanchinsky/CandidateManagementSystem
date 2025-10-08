using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(c => c.CandidateData.SocialNetworks)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Employee>> SearchByNameAsync(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return new List<Employee>();

        var searchTerms = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(term => term.ToLower())
                                  .ToList();

        var query = _context.Employees.AsQueryable();

        foreach (var term in searchTerms)
        {
            query = query.Where(e =>
                e.CandidateData.FullName.FirstName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                e.CandidateData.FullName.LastName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                (e.CandidateData.FullName.Patronymic != null && e.CandidateData.FullName.Patronymic.Contains(term, StringComparison.CurrentCultureIgnoreCase)));
        }

        return await query.ToListAsync();
    }
}