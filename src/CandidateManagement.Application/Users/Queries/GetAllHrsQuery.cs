using CandidateManagement.Application.Users.Dtos;
using MediatR;

namespace CandidateManagement.Application.Users.Queries;

public sealed record GetAllHrsQuery(
) : IRequest<List<UserReadDto>>;