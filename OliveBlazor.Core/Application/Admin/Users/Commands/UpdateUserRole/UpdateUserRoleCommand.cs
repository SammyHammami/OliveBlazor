using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommand : IRequest<Unit>, IRequirePermission, IAnnounceCompletion
{
    public string UserId { get; set; }
    public string Role { get; set; }
    public IEnumerable<Permissions> RequiredPermissions => new[] { Permissions.AssignRoles };
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Unit>
{
    private readonly IIdentityService _identityService;
    private readonly IPermissionService _permissionService;

    public UpdateUserRoleCommandHandler(IIdentityService identityService, IPermissionService permissionService)
    {
        _identityService = identityService;
        _permissionService = permissionService;
    }
    public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        bool roleExists = await _identityService.RoleExistsAsync(request.Role);
        if (!roleExists)
        {

            // Create the role if you want
            await _identityService.AddRole(request.Role, cancellationToken);
        }

        await _identityService.UpdateUserRoleAssignment(request.Role, request.UserId, cancellationToken, permissionService:_permissionService);

        return default;
    }
}