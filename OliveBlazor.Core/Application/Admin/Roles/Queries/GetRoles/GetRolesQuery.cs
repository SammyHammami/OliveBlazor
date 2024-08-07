using MediatR;
using OliveBlazor.Core.Application.Services.IdentityManagement;

namespace OliveBlazor.Core.Application.Admin.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<RolesVm>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, RolesVm>
{
    private readonly IIdentityService _identityService;

    public GetRolesQueryHandler(IIdentityService identityService)
    {
       
        _identityService = identityService;
    }
    public async Task<RolesVm> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _identityService.GetAllRolesAsync(cancellationToken);

        var rolesVm = new RolesVm()
        {
            Roles = roles
        };
        return rolesVm;
    }
}