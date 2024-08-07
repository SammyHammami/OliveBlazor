using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Permission.Queries.GetPermissionMatrix;

public class PermissionsMatrixViewModel
{
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    public List<Permissions> PermissionsList { get; set; } = new List<Permissions>();
    public Dictionary<Permissions, List<RoleDto>> Matrix { get; set; } = new Dictionary<Permissions, List<RoleDto>>();

}