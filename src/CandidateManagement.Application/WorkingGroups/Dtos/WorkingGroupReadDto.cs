using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Users.Dtos;

namespace CandidateManagement.Application.WorkingGroups.Dtos;

public sealed record WorkingGroupReadDto(
    Guid Id,
    string Name,
    IReadOnlyCollection<UserReadDto> Participants,
    IReadOnlyCollection<CandidateReadDto> Candidates
);