using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OliveBlazor.Core.Application.Admin.Users.Queries.UserList;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.UI.Pages.Admin.Users;

public partial class Index
{
    [Inject] public IIdentityService? UserService { get; set; }

    [Inject]
    public required IMediator Mediator { get; set; }


    [Inject]
    public IAuthorizationService AuthorizationService { get; set; }


    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private bool _canManageUsers;

    private ClaimsPrincipal _currentUser;


    public UsersVm? Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _currentUser = authState.User;


        _canManageUsers = _currentUser.HasClaim(c => c.Type == "Permission" && c.Value == Permissions.ManageUsers.ToPolicy());
        var authResult = _currentUser.HasClaim(c => c.Type == "Permission" && c.Value == Permissions.ViewUsers.ToPolicy());
        if (!authResult)
        {
            // Handle the lack of authorization: redirect, display a message, etc.
        }
        else
        {
            var query = new UserListQuery();
            Model = await Mediator.Send(query);

        }


        

    }

    private IEnumerable<Permissions> GetPermissionsFromClaims(ClaimsPrincipal user)
    {
        return user.Claims
            .Where(c => c.Type == "Permission") // Replace with your actual claim type
            .Select(c => Enum.Parse<Permissions>(c.Value))
            .ToList();
    }

}