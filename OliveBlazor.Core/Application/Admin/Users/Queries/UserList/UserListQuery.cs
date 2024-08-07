using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Users.Queries.UserList;

public record UserListQuery() : IRequest<UsersVm>, IRequirePermission
{
    public IEnumerable<Permissions> RequiredPermissions => new[] { Permissions.ViewUsers };
}

public class GetUsersQueryHandler : IRequestHandler<UserListQuery, UsersVm>
{
    public GetUsersQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    private readonly IIdentityService _identityService;

    public async Task<UsersVm> Handle(UserListQuery request, CancellationToken cancellationToken)
    {
        var result = new UsersVm
        {
            Users = await _identityService.GetUsersAsync(cancellationToken)
        };

        return result;
    }
}