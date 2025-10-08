using CandidateManagement.Domain.Enums;

namespace CandidateManagement.Application.Services;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Role? UserRole { get; }
    Guid? WorkGroupId { get; }
}