using CandidateManagement.Application.Auth.Dtos;
using CandidateManagement.Application.Services;
using MapsterMapper;
using MediatR;

namespace CandidateManagement.Application.Auth.Commands;

public class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand, TokensDto>
{
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public RefreshTokensCommandHandler(ITokenService tokenService, IMapper mapper)
    {
        _tokenService = tokenService;
        _mapper = mapper;
    }
    public async Task<TokensDto> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokensAsync(_mapper.Map<TokensDto>(request));
    }
}