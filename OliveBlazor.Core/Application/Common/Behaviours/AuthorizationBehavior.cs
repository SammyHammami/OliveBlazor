using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Application.Services.IdentityManagement;

namespace OliveBlazor.Core.Application.Common.Behaviours;

public class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IErrorService _errorService;

    public AuthorizationBehavior(IPermissionService permissionService, ICurrentUserService currentUserService, IErrorService errorService)
    {
        _permissionService = permissionService;
        _currentUserService = currentUserService;
        _errorService = errorService;
        // Assign dependencies to private fields
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            if (request is IRequirePermission requirePermission)
            {
                var requiredPermissions = requirePermission.RequiredPermissions;
                var userId = _currentUserService.UserId;

                foreach (var permission in requiredPermissions)
                {
                    if (!await _permissionService.HasPermissionAsync(userId, permission, cancellationToken))
                    {
                        throw new UnauthorizedAccessException($"User does not have required permission: {permission}");
                    }
                }
            }

            // Proceed to the next behavior in the pipeline
            return await next();
        }

        catch (UnauthorizedAccessException ex)
        {
            await _errorService.TransferToUnauthorizedAccessPage(ex.Message);
            return default;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

      
    }
}
