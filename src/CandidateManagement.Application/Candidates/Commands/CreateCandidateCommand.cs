using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.DTOs;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Domain.ValueObjects;
using MediatR;

namespace CandidateManagement.Application.Candidates.Commands;

public sealed record CreateCandidateCommand(
    FullName FullName,
    Email Email,
    PhoneNumber PhoneNumber,
    string Country,
    DateTime DateOfBirth,
    List<SocialNetworkCreateDto> SocialNetworks,
    WorkSchedule WorkSchedule,
    Guid CreatedById
) : IRequest<CandidateReadDto>;