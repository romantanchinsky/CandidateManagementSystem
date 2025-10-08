using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.DTOs;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Domain.ValueObjects;
using MediatR;

namespace CandidateManagement.Application.Candidates.Commands
{
    public record UpdateCandidateCommand(
        Guid CandidateId,
        FullName FullName,
        Email Email,
        PhoneNumber PhoneNumber,
        string Country,
        DateTime DateOfBirth,
        WorkSchedule WorkSchedule,
        List<SocialNetworkCreateDto> SocialNetworks,
        Guid CurrentUserId
    ) : IRequest<CandidateReadDto>;
}