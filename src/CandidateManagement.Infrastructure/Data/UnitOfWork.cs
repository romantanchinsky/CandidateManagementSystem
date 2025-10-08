using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CandidateManagement.Infrastructure.Data;
using CandidateManagement.Application.Interfaces;
using System.Data;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;

        private Lazy<IUserRepository> _lazyUserRepository;
        private Lazy<IWorkingGroupRepository> _lazyWorkingGroupRepository;
        private Lazy<ICandidateRepository> _lazyCandidateRepository;
        private Lazy<IEmployeeRepository> _lazyEmployeeRepository;
        private Lazy<IVerificationRepository> _lazyVerificationRepository;
        private Lazy<ITokenRepository> _lazyTokenRepository;

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            ICandidateRepository candidateRepository,
            IEmployeeRepository employeeRepository,
            IVerificationRepository verificationRepository,
            ITokenRepository tokenRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _lazyUserRepository = new Lazy<IUserRepository>(() => userRepository);
            _lazyWorkingGroupRepository = new Lazy<IWorkingGroupRepository>(() => workingGroupRepository);
            _lazyCandidateRepository = new Lazy<ICandidateRepository>(() => candidateRepository);
            _lazyEmployeeRepository = new Lazy<IEmployeeRepository>(() => employeeRepository);
            _lazyVerificationRepository = new Lazy<IVerificationRepository>(() => verificationRepository);  
            _lazyTokenRepository = new Lazy<ITokenRepository>(() => tokenRepository);
        }

        public IUserRepository UserRepository => _lazyUserRepository.Value;
        public IWorkingGroupRepository WorkGroupRepository => _lazyWorkingGroupRepository.Value;
        public ICandidateRepository CandidateRepository => _lazyCandidateRepository.Value;
        public IEmployeeRepository EmployeeRepository => _lazyEmployeeRepository.Value;
        public IVerificationRepository VerificationRepository => _lazyVerificationRepository.Value;
        public ITokenRepository TokenRepository => _lazyTokenRepository.Value;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new UnitOfWorkDomainException("Concurrency conflict occurred");
            }
            catch (DbUpdateException ex)
            {
                throw new UnitOfWorkDomainException("DATABASE_UPDATE_ERROR");
            }
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction to commit");
            }

            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction to rollback");
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }
        public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Database.CanConnectAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currentTransaction?.Dispose();
                _context?.Dispose();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}