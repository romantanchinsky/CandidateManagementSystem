using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.WorkingGroups.Commands;

public class CreateWorkingGroupCommandHandler : IRequestHandler<CreateWorkingGroupCommand, WorkingGroup>
{
    IWorkingGroupRepository _workingGroupRepository;
    private readonly IUserRepository _userRepository;

    public CreateWorkingGroupCommandHandler(
        IWorkingGroupRepository workingGroupRepository,
        IUserRepository userRepository)
    {
        _workingGroupRepository = workingGroupRepository;
        _userRepository = userRepository;
    }
    public async Task<WorkingGroup> Handle(CreateWorkingGroupCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsAdmin(request.CurrentUserId))
        {
            throw new AccessDeniedDomainException("Only admin can create user");
        }
        var existingWorkingGroup = await _workingGroupRepository.GetByNameAsync(request.Name);
        if (existingWorkingGroup is not null)
        {
            throw new ConflictDomainException($"Working group with name: {request.Name} already exists");
        }
        var workingGroup = request.Adapt<WorkingGroup>();
        await _workingGroupRepository.AddAsync(workingGroup);
        return workingGroup;
    }
}