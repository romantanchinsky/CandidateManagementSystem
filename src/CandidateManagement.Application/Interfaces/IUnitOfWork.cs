using System.Data;

namespace CandidateManagement.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IWorkingGroupRepository WorkGroupRepository { get; }
        ICandidateRepository CandidateRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IVerificationRepository VerificationRepository { get; }
        ITokenRepository TokenRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        
        Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
    }
}