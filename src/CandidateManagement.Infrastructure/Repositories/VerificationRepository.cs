using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Verification?> GetByIdAsync(Guid id)
        {
            return await _context.Verifications
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Verification verification)
        {
            await _context.Verifications.AddAsync(verification);
            await _context.SaveChangesAsync();
        }
    }
}