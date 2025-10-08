namespace CandidateManagement.Application.Users.Dtos;

public sealed record UserReadDto(
    Guid Id,
    string Role,
    string FirstName,
    string LastName,
    string? Patronymic,
    string Login,
    Guid? WorkingGroupId 
);