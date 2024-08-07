using System.Security.Claims;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Domain.Accounts;

namespace OliveBlazor.UI.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        UserId = currentUser?.FindFirstValue(ClaimTypes.NameIdentifier);
        UserName = currentUser?.Identity?.Name;
        IsAuthenticated = UserId != null;
    }

    public string UserId { get; }
    public string UserName { get; }
    public bool IsAuthenticated { get; }
    public string CurrentPath { get; }
    public OliveUser User { get; set; }
}