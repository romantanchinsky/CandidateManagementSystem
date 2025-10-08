using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Candidates.Commands;

public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, CandidateReadDto>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IUserRepository _userRepository;

    public UpdateCandidateCommandHandler(ICandidateRepository candidateRepository, IUserRepository userRepository)
    {
        _candidateRepository = candidateRepository;
        _userRepository = userRepository;
    }
    public async Task<CandidateReadDto> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId) ?? throw new NotFoundDomainException($"Candidate {request.CandidateId} not found.");

        await ValidateUserAccessAsync(request.CurrentUserId, candidate.WorkingGroupId);

        var candidateData = request.Adapt<CandidateData>();
        candidate.UpdateData(candidateData);
        candidate.UpdateWorkSchedule(request.WorkSchedule);

        await _candidateRepository.UpdateAsync(candidate);
        return candidate.Adapt<CandidateReadDto>();
    }

    private async Task ValidateUserAccessAsync(Guid userId, Guid candidateWorkGroupId)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundDomainException($"User with id: {userId} not found.");

        if (user.IsAdmin())
            return;

        if (user.IsHR() && user.WorkingGroupId == candidateWorkGroupId)
            return;

        throw new AccessDeniedDomainException("Only Administrator can update user from another group.");
    }
}