using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Permission.Queries.GetPermissionMatrix;

public class GetPermissionMatrixQuery :IRequest<PermissionsMatrixViewModel>, IRequirePermission
{
    public IEnumerable<Permissions> RequiredPermissions => new[] { Permissions.ManagePermissions };
}

public class GetPermissionMatrixQueryHandler : IRequestHandler<GetPermissionMatrixQuery, PermissionsMatrixViewModel>
{

    private readonly IIdentityService _identityService;
    public GetPermissionMatrixQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<PermissionsMatrixViewModel> Handle(GetPermissionMatrixQuery request, CancellationToken cancellationToken)
    {
        var vm = new PermissionsMatrixViewModel();
        vm. Roles  = await _identityService.GetAllRolesAsync(cancellationToken);
        vm.PermissionsList = Enum.GetValues(typeof(Permissions)).Cast<Permissions>().ToList();
        foreach (var permission in vm.PermissionsList)
        {
            vm.Matrix[permission] = vm.Roles.Where(r => r.Permissions.HasFlag(permission)).ToList();
        }

        return vm;
    }
}