using Banking.Application.Interfaces.Services;
using System.Security.Claims;

namespace Banking.Infrastructure.Services;

public class UserContextService : IUserContextService // TODO: implement
{
    //private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(/*IHttpContextAccessor httpContextAccessor*/)
    {
        //_httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => null; // _httpContextAccessor.HttpContext?.User;

    public string? GetUserId() =>
        User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? GetUserName() =>
        User?.Identity?.Name;

    public string[] GetRoles() =>
        User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToArray() ?? Array.Empty<string>();

    public bool IsInRole(string role) =>
        User?.IsInRole(role) ?? false;
}
