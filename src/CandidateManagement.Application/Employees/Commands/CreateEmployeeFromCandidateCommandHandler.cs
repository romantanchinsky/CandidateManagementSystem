using CandidateManagement.Application.Employees.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Employees.Commands;

public sealed class CreateEmployeeFromCandidateCommandHandler
    : IRequestHandler<CreateEmployeeFromCandidateCommand, EmployeeReadDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeFromCandidateCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeReadDto> Handle(CreateEmployeeFromCandidateCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(request.CandidateId);
            if (candidate == null)
            {
                throw new NotFoundDomainException($"Candidate with id: {request.CandidateId} not found");
            }

            await ValidateUserAccessAsync(request.CurrentUserId, candidate.WorkingGroupId);

            var employee = new Employee(candidate, DateTime.UtcNow);
            await _unitOfWork.EmployeeRepository.AddAsync(employee);

            await _unitOfWork.CandidateRepository.Delete(candidate);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return employee.Adapt<EmployeeReadDto>();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task ValidateUserAccessAsync(Guid userId, Guid candidateWorkGroupId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null) throw new NotFoundDomainException($"User with id: {userId} not found");

        if (user.IsAdmin()) return;
        if (user.IsHR() && user.BelongsToWorkingGroup(candidateWorkGroupId)) return;

        throw new AccessDeniedDomainException("Only administrator can work with candidates from another group");
    }
}