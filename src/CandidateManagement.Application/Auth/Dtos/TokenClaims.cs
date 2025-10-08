using CandidateManagement.Domain.Enums;

namespace CandidateManagement.Application.Auth.Dtos;

public sealed record TokenClaims(
    Guid UserId,
    Role UserRole,
    Guid? WorkingGroupId
);