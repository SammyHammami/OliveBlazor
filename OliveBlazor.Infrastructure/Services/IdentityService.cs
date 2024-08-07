using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Application.Admin.Users.Queries.Common;
using OliveBlazor.Core.Application.Entities;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Accounts;
using OliveBlazor.Core.Domain.Security;
using OliveBlazor.Infrastructure.Data;
using OliveBlazor.Infrastructure.Indentity;

namespace OliveBlazor.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly RoleManager<RoleIdentity> _roleManager;
    private readonly ApplicationDbContext _context;

    public IdentityService(SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager,
        RoleManager<RoleIdentity> roleManager, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        
    }


    public async Task<OperationResult<UserRegistrationResult>> CreateUserAsync(UserRegistrationDto dto)
    {
        var user = CreateUser(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);

        // You might have a Result class to represent the outcome of the operation
        // Convert the result to your custom Result type and return it

        var operationResult = new OperationResult<UserRegistrationResult> { Success = result.Succeeded, Errors = result.Errors.Select(e => e.Description) };

        var userId = await _userManager.GetUserIdAsync(user);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        operationResult.Data = new UserRegistrationResult()
        {
            UserId = userId,
            EmailConfirmationToken = token
        };
        return operationResult;
    }

    private UserIdentity CreateUser(UserRegistrationDto dto)
    {
        try
        {
            var oliveUser = new OliveUser()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            var user = new UserIdentity { UserName = dto.UserName, Email = dto.Email, OliveUser = oliveUser };
            return user;

        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(UserIdentity)}'. " +
                                                $"Ensure that '{nameof(UserIdentity)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
    public static IEnumerable<Permissions> GetActivePermissions(Permissions permissions)
    {
        foreach (Permissions permission in Enum.GetValues(typeof(Permissions)))
        {
            if (permissions.HasFlag(permission))
            {
                yield return permission;
            }
        }
    }
    public async Task SignInAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            //    var userClaims = await GetUserClaims(user);
            //    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Assuming 'user' is the user you are signing in
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>();

            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var oliveRole = _context.OliveRoles.FirstOrDefault(r => r.IdentityRoleId == role.Id);

                    // Assuming you have a method to get permissions from OliveRole
                    var activePermissions = GetActivePermissions(role.OliveRole.Permissions);

                    foreach (var permission in activePermissions)
                    {
                        // Creates a claim for each active permission
                        userClaims.Add(new Claim("Permission", permission.ToString()));
                    }
                }
            }

            // Add the claims to the user
            foreach (var claim in userClaims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
        }
    }

    private async Task<List<Claim>> GetUserClaims(UserIdentity user)
    {
        // Retrieve user roles and corresponding permissions here
        // This is a simplified example, you'll need to implement the logic
        // to get the actual permissions from the user's roles
        var permissions = new List<string> { "Permission1", "Permission2" }; // Replace with actual permission fetching logic
        return permissions.Select(p => new Claim("Permission", p)).ToList();
    }

    public async Task<OperationResult<string>> LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
        return new OperationResult<string> { Success = true };
    }

    public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        List<UserDto> users = await _userManager.Users
            .OrderBy(r => r.UserName)
            .Select(u => new UserDto(u.Id, u.UserName ?? string.Empty, u.Email ?? string.Empty, u.EmailConfirmed))
            .ToListAsync(cancellationToken);
        return users;
    }

    public async Task<UserDto> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _userManager.GetRolesAsync(user);

        var dto = new UserDto
            (user.Id, user.UserName, user.Email, user.EmailConfirmed);
        foreach (var role in roles)
        {

            dto.Roles.Add(role);

        }

        return dto;
    }


    public async Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {

        //var roles = await _roleManager.Roles

        //    .OrderBy(r => r.Id)
        //    .Select(r => new RoleDto(r.Id, r.Name))
        //    .ToListAsync(cancellationToken);


        var roles = await _context.OliveRoles.OrderBy(r => r.Id)
                .Select(r => new RoleDto(r.Id, r.Name, r.IdentityRoleId, r.Permissions))
                .ToListAsync(cancellationToken);

        ;
        return roles;
    }

    public async Task AddRole(RoleDto roleDto, CancellationToken? cancellationToken=null)
    {
        var oliveRole = new OliveRole()
        {
            Id = roleDto.Id,
            Name = roleDto.Name,

        };

        var role = new RoleIdentity()
        {
            Name = roleDto.Name,
            Id = roleDto.Id.ToString(),
            OliveRole = oliveRole
        };

        await _roleManager.CreateAsync(role);
        
    }
    public async Task AddRole(string roleName, CancellationToken cancellationToken)
    {

        var oliveRole = new OliveRole()
        {
            Name = roleName

        };
        var role = new RoleIdentity()
        {
            Name = roleName,
            Id = Guid.NewGuid().ToString(),
            OliveRole = oliveRole
        };

        await _roleManager.CreateAsync(role);
    }

    public async Task RemoveRole(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _context.OliveRoles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
        
        if (role != null)
        {
            if (role.Name=="Admin")
            {
                throw new InvalidOperationException($"You can't delete Admin role");

            }
            var identityRole = await _roleManager.FindByIdAsync(role.IdentityRoleId);
            if (identityRole != null)
            {
                var identityResult = await _roleManager.DeleteAsync(identityRole);
                if (!identityResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to delete identity role with Id {role.IdentityRoleId}");
                }
            }
        }
    }



    public async Task<bool> RoleExistsAsync(string roleName)
    {
        bool roleExists = await _roleManager.RoleExistsAsync(roleName);
        return roleExists;
    }

    public async Task UpdateUserRoleAssignment(string role, string userId, CancellationToken cancellationToken, IPermissionService permissionService)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            bool isInRole = await _userManager.IsInRoleAsync(user, role);
            if (isInRole)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role);
            }
            else
            {
                var result = await _userManager.AddToRoleAsync(user, role);

            }
            await permissionService.InvalidateCacheForUser(userId);
        }
    }

    public async Task<bool> UpdateRolePermissions(OliveRole role, IPermissionService permissionService)
    {
        var dbOliveRole = await _context.OliveRoles
            .FirstOrDefaultAsync(or => or.IdentityRoleId == role.IdentityRoleId);

        if (dbOliveRole != null)
        {
            // Update the role's permissions
            dbOliveRole.Permissions = role.Permissions;
            await _context.SaveChangesAsync();

            // Get the IdentityRole object to find the associated users
            var identityRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == role.IdentityRoleId);

            // Find all users in this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(identityRole.Name);

            foreach (var user in usersInRole)
            {
                // Update security stamp to force sign in on next request
                await _userManager.UpdateSecurityStampAsync(user);
                // If you want to force immediate sign out, uncomment the following line
             
                //Invalidate Cache
                await permissionService.InvalidateCacheForUser(user.Id);
            }
           
            return true;
        }
        return false;
    }

    public async Task<SignInResponse> SignInAsync(SignInModel signInModel)
    {
        var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, lockoutOnFailure: false);

        var response = new SignInResponse()
        {
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor,
            Succeeded = result.Succeeded
        };
        if (response.Succeeded)
        {
            // Get the user
            var user = await _userManager.FindByNameAsync(signInModel.Email);

            // Get roles and add role claims
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>();
            var existingClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in existingClaims.Where(c => c.Type == ClaimTypes.Role || c.Type == "Permission"))
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            foreach (var roleName in userRoles)
            {
                // Add the role claim
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var oliveRole = _context.OliveRoles.FirstOrDefault(r => r.IdentityRoleId == role.Id);
                    var activePermissions = GetActivePermissions(oliveRole.Permissions);
                    foreach (var permission in activePermissions)
                    {
                        // Add the permission claim
                        var permissionClaim = new Claim("Permission", permission.ToString());
                        await _userManager.AddClaimAsync(user, permissionClaim);
                    }
                }
            }

            // Refresh the sign-in to apply the changes
            await _signInManager.RefreshSignInAsync(user);
           
        }



        return response;
    }
}