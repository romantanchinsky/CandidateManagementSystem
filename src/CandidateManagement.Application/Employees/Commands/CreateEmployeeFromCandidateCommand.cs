using CandidateManagement.Application.Employees.Dtos;
using MediatR;

namespace CandidateManagement.Application.Employees.Commands;

public record CreateEmployeeFromCandidateCommand
(
    Guid CandidateId,
    Guid CurrentUserId
) : IRequest<EmployeeReadDto>;