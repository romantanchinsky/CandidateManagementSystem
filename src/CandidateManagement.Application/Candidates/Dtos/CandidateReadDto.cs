using CandidateManagement.Application.DTOs;
using CandidateManagement.Domain.Enums;

namespace CandidateManagement.Application.Candidates.Dtos;

public sealed record CandidateReadDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string PhoneNumber,
    string Country,
    DateTime DateOfBirth,
    List<SocialNetworkCreateDto> SocialNetworks,
    string WorkSchedule,
    DateTime LastUpdated,
    Guid WorkingGroupId,
    Guid CreatedById
);