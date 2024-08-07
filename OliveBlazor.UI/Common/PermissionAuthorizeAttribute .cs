using Microsoft.AspNetCore.Authorization;
using OlivePatterns.Core.Domain.Security;

namespace ServerBlazorIdentity.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(Permissions permission)
        : base(permission.ToString())
    { }
}
