using CandidateManagement.Application.DTOs;

namespace CandidateManagement.Application.Employees.Dtos;

public sealed record EmployeeReadDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string PhoneNumber,
    string Country,
    DateTime DateOfBirth,
    List<SocialNetworkCreateDto> SocialNetworks,
    DateTime HireData
);