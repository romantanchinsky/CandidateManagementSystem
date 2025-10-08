using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Interfaces;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Candidates.Commands;

public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CandidateReadDto>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IWorkingGroupRepository _workingGroupRepository;
    private readonly IUserRepository _userRepository;

    public CreateCandidateCommandHandler(
        ICandidateRepository candidateRepository,
        IWorkingGroupRepository workingGroupRepository,
        IUserRepository userRepository
    )
    {
        _candidateRepository = candidateRepository;
        _workingGroupRepository = workingGroupRepository;
        _userRepository = userRepository;
    }
    public async Task<CandidateReadDto> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var hrUser = await _userRepository.GetByIdAsync(request.CreatedById);
        if (hrUser is null || hrUser.Role != Role.HR)
        {
            throw new NotFoundDomainException($"HR user with id: {request.CreatedById }not found");
        }
        if (hrUser.WorkingGroupId is null)
        {
            throw new WorkingGroupDomainException("Hr cannot create a candidate while he does not belong to any Working group");
        }

        var candidateData = new CandidateData(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.Country,
            request.DateOfBirth);

        foreach (var sn in request.SocialNetworks)
        {
            candidateData.AddSocialNetwork(sn.Username, sn.Type);
        }
        var candidate = new Candidate(
            candidateData,
            request.WorkSchedule,
            (Guid)hrUser.WorkingGroupId,
            request.CreatedById);

        await _candidateRepository.AddAsync(candidate);

        return candidate.Adapt<CandidateReadDto>();
    }
}