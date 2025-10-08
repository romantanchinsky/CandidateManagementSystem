using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Services;
using CandidateManagement.Application.Users.Dtos;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Domain.Enums;
using Mapster;
using MapsterMapper;
using MediatR;

namespace CandidateManagement.Application.Users.Commands;

public class CreateHrCommandHandler : IRequestHandler<CreateHrCommand, UserReadDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public CreateHrCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    public async Task<UserReadDto> Handle(CreateHrCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsAdmin(request.CurrentUserId))
        {
            throw new AccessDeniedDomainException("Only administrator can create user");
        }
        var existedUser = await _userRepository.GetByLoginAsync(request.Login);
        if (existedUser is not null)
        {
            throw new ConflictDomainException($"User with Login: {request.Login} already exists");
        }
        var user = new User(
            Role.HR,
            request.FullName,
            request.Login,
            _passwordHasher.HashPassword(request.Password)
        );
        await _userRepository.AddAsync(user);
        return user.Adapt<UserReadDto>();
    }
}