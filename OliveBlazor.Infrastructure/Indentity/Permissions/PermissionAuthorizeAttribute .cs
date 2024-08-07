using Microsoft.AspNetCore.Authorization;

namespace OliveBlazor.Infrastructure.Indentity.Permissions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(OliveBlazor.Core.Domain.Security.Permissions permission)
        : base(permission.ToString())
    { }
}
