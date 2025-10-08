using CandidateManagement.Application.Employees.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Employees.Queries;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeReadDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<EmployeeReadDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _employeeRepository.GetByIdAsync(request.Id) ?? throw new NotFoundDomainException($"Employee with Id:{request.Id} not found");
        return result.Adapt<EmployeeReadDto>();
        
    }
}