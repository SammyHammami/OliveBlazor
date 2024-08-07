using Microsoft.AspNetCore.Authorization;

namespace OliveBlazor.Infrastructure.Indentity.Permissions;

public class EnumRequirementHandler : AuthorizationHandler<EnumRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EnumRequirement requirement)
    {
        if (context.User.HasClaim("Permission", requirement.RequiredPermission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
