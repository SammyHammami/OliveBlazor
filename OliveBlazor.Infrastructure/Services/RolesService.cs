using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OlivePatterns.Core.Application.Admin.Roles;
using OlivePatterns.Core.Application.Admin.Roles.Commands.AddRole;
using OlivePatterns.Core.Application.Admin.Users.Queries.Common;
using OlivePatterns.Core.Application.Services.RolesManagement;

namespace OlivePatterns.Infrastructure.Services;

public class RolesService : IRolesService
{
    private readonly RoleManager<IdentityRole> _roleManager;


    public RolesService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {

        var roles = await _roleManager.Roles

            .OrderBy(r => r.Id)
            .Select(r => new RoleDto(r.Id, r.Name))
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task AddRole(RoleDto roleDto, CancellationToken cancellationToken)
    {
        var role = new IdentityRole(roleDto.Name)
        {
            Id = roleDto.Id
        };

        await _roleManager.CreateAsync(role);
    }

   
}