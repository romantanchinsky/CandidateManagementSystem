using CandidateManagement.Application.Employees.Dtos;
using MediatR;

namespace CandidateManagement.Application.Employees.Queries;

public sealed record GetEmployeeByIdQuery(
    Guid Id
) : IRequest<EmployeeReadDto>;