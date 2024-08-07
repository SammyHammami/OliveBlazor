using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Users.Queries.UserDetails;

public class UserDetailsQuery: IRequest<UserDetailsVm> ,IRequirePermission
{
    public string UserId { get; set; }
    public IEnumerable<Permissions> RequiredPermissions  => new[] { Permissions.ManageUsers };
}

public class UserDetailsQueryHandler : IRequestHandler<UserDetailsQuery, UserDetailsVm>
{
    private readonly IIdentityService _identityService;

    // private readonly UserManager<IdentityUser> _userManager;
    // private readonly RoleManager<IdentityRole> _roleManager;

    // public UserDetailsQueryHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    // {
    //     // _userManager = userManager;
    //     // _roleManager = roleManager;
    // }

    public UserDetailsQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
      
    }
    
    
    public async Task<UserDetailsVm> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserById(request.UserId);

        var allRoles = await _identityService.GetAllRolesAsync(cancellationToken);

        var roles = allRoles.Select(r => r.Name).ToList();

        return new UserDetailsVm
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            UserRoles = user.Roles??new List<string>(),
            AllRoles = roles
        };
    }
}