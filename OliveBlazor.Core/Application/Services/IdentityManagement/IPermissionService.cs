using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Services.IdentityManagement;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, Permissions permission, CancellationToken cancellationToken);
    Task InvalidateCacheForUser(string userId);

}