using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Application.Admin.Users.Queries.Common;
using OliveBlazor.Core.Application.Entities;
using OliveBlazor.Core.Domain.Accounts;

namespace OliveBlazor.Core.Application.Services.IdentityManagement;

public interface IIdentityService
{
    Task<OperationResult<UserRegistrationResult>> CreateUserAsync(UserRegistrationDto dto);
    Task SignInAsync(string userId);
    Task<OperationResult<string>> LogoutUserAsync();
    Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
    Task<UserDto> GetUserById(string userId);

    Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken);
    Task AddRole(RoleDto roleDto, CancellationToken? cancellationToken = null);
    Task AddRole(string roleName, CancellationToken cancellationToken);
    Task RemoveRole(Guid roleId, CancellationToken cancellationToken);

    Task<bool> RoleExistsAsync(string roleName);
    Task UpdateUserRoleAssignment(string role, string userId, CancellationToken cancellationToken, IPermissionService permissionService);
    Task<bool> UpdateRolePermissions(OliveRole role, IPermissionService permissionService);

    Task<SignInResponse> SignInAsync(SignInModel signInModel);
}