using Microsoft.AspNetCore.Authorization;

namespace OliveBlazor.Infrastructure.Indentity.Permissions;

public class EnumRequirement : IAuthorizationRequirement
{
    public string RequiredPermission { get; }

    public EnumRequirement(string requiredPermission)
    {
        RequiredPermission = requiredPermission;
    }
}
