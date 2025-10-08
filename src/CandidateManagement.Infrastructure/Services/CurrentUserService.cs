using CandidateManagement.Application.Services;
using CandidateManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CandidateManagement.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid? UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            return id != null ? Guid.Parse(id) : null;
        }
    }

    public Role? UserRole
    {
        get
        {
            var role = _httpContextAccessor.HttpContext?.User?.FindFirst("userRole")?.Value;
            return role != null ? Enum.Parse<Role>(role) : null;
        }
    }

    public Guid? WorkGroupId
    {
        get
        {
            var workGroupId = _httpContextAccessor.HttpContext?.User?.FindFirst("workGroupId")?.Value;
            return workGroupId != null ? Guid.Parse(workGroupId) : null;
        }
    }
}