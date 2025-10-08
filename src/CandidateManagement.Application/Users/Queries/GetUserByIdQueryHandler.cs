using CandidateManagement.Application.Interfaces;
using CandidateManagement.Application.Users.Dtos;
using Mapster;
using MediatR;

namespace CandidateManagement.Application.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetuserByIdQuery, UserReadDto>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserReadDto> Handle(GetuserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id) ?? throw new NotFoundDomainException($"User with id:{request.Id} not found");
        return user.Adapt<UserReadDto>();
    }
}