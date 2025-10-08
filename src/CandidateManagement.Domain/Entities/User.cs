using CandidateManagement.Domain.Enums;
using CandidateManagement.Domain.Interfaces;
using CandidateManagement.Domain.ValueObjects;

namespace CandidateManagement.Domain.Entities;

public class User : IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Role Role { get; private set; }
    public FullName FullName { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public Guid? WorkingGroupId { get; private set; } = null;

    private User() { }

    public User(
        Role role,
        FullName fullName,
        string login,
        string passwordHash,
        Guid? workingGroupId = null
    )
    {
        Role = role;
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Login = !string.IsNullOrWhiteSpace(login) ? login.ToLower() : throw new ArgumentException("Login required", nameof(login));
        PasswordHash = !string.IsNullOrWhiteSpace(passwordHash) ? passwordHash : throw new ArgumentException("PasswordHash required", nameof(passwordHash));
        WorkingGroupId = null;
    }

    public void AssignToWorkingGroup(Guid? workingGroupId)
    {
        if (Role != Role.HR)
        {
            throw new UserDomainException("Only users with the HR role can be assigned to the work group.");
        }
        WorkingGroupId = workingGroupId;
    }

    public void RemoveFromWorkGroup()
    {
        WorkingGroupId = null;
    }

    public bool IsAdmin()
    {
        return Role == Role.Admin;
    }

    public bool IsHR()
    {
        return Role == Role.HR;
    }

    public bool BelongsToWorkingGroup(Guid workingGroupId)
    {
        return WorkingGroupId == workingGroupId;
    }
}