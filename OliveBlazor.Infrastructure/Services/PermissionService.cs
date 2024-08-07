using Microsoft.Extensions.Caching.Memory;
using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;
using OliveBlazor.Infrastructure.Data;

namespace OliveBlazor.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IIdentityService _identityService;
    private readonly IMemoryCache _cache;

    public PermissionService(ApplicationDbContext dbContext, IIdentityService identityService, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _identityService = identityService;
        _cache = cache;
    }

    public async Task<bool> HasPermissionAsync(string userId, Permissions permission, CancellationToken cancellationToken)
    {
        var cacheKey = $"PermissionsForUser_{userId}";

        List<Permissions> permissions;
        List<RoleDto> roles;
        if (!_cache.TryGetValue(cacheKey, out List<Permissions> cachedPermissions))
        {
            var user = await _identityService.GetUserById(userId);

            //   permissions = Enum.GetValues(typeof(Permissions)).Cast<Permissions>().ToList();
            permissions = new List<Permissions>();
            roles = await _identityService.GetAllRolesAsync(cancellationToken);
            roles = roles.Where(r => user.Roles.Contains(r.Name)).ToList();
            foreach (var role in roles)
            {


                permissions.Add(role.Permissions);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Adjust as needed
            _cache.Set(cacheKey, permissions, cacheEntryOptions);
            cachedPermissions = permissions;

            // Check if any of the roles include the specified permission
            //  return roles.Any(role => role.Permissions.HasFlag(permission));
        }

        return cachedPermissions.Any(p => p.HasFlag(permission));



    }

    public async Task InvalidateCacheForUser(string userId)
    {
        var cacheKey = $"PermissionsForUser_{userId}";
        _cache.Remove(cacheKey);
    }
}
