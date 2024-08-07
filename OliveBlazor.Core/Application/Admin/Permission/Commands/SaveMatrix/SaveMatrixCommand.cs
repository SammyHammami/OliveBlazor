using MediatR;
using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Permission.Commands.SaveMatrix;

public class SaveMatrixCommand : IRequest,IRequirePermission, IAnnounceCompletion
{
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    public IEnumerable<Permissions> RequiredPermissions => new[] { Permissions.ManagePermissions };
}

public class SaveMatrixCommandHandler : IRequestHandler<SaveMatrixCommand >
{
    
    private readonly IIdentityService _identityService;
    private readonly IPermissionService _permissionService;

    public SaveMatrixCommandHandler(IIdentityService identityService, IPermissionService permissionService)
    {
        _identityService = identityService;
        _permissionService = permissionService;
    }

    public async Task Handle(SaveMatrixCommand request, CancellationToken cancellationToken)
    {
        foreach (var role in request.Roles)
        {
            await _identityService.UpdateRolePermissions(role, _permissionService);
        }
    }
}