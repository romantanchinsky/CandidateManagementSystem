using CandidateManagement.Application.Users.Dtos;
using MediatR;
using CandidateManagement.Domain.Enums;
using CandidateManagement.Application.Interfaces;
using Mapster;

namespace CandidateManagement.Application.Users.Queries;

public class GetAllHrsQueryHandler : IRequestHandler<GetAllHrsQuery, List<UserReadDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllHrsQueryHandler(IUserRepository context)
    {
        _userRepository = context;
    }
    public async Task<List<UserReadDto>> Handle(GetAllHrsQuery request, CancellationToken cancellationToken)
    {
        var hrs = await _userRepository.GetAllHrsAsync();
        return hrs.Adapt<List<UserReadDto>>();
    }
}