using MediatR;
using Microsoft.AspNetCore.Components;
using OliveBlazor.Core.Application.Admin.Users.Commands.UpdateUserRole;
using OliveBlazor.Core.Application.Admin.Users.Queries.UserDetails;

namespace OliveBlazor.UI.Pages.Admin.Users;

public partial class UserDetails
{
    [Inject]
    public IMediator Mediator { get; set; }

    [Parameter]
    public string UserId { get; set; }

    private UserDetailsVm userDetails;

    protected override async Task OnParametersSetAsync()
    {
        userDetails = await Mediator.Send(new UserDetailsQuery { UserId = UserId });
    }

    private async Task UpdateUserRoles()
    {
        // TODO: Call another MediatR command to update the user's roles based on the selected checkboxes
    }

    private async Task UpdateUserRole(string roleName, bool isChecked)
    {
        if (isChecked && !userDetails.UserRoles.Contains(roleName))
        {
            userDetails.UserRoles.Add(roleName);
        }
        else if (!isChecked && userDetails.UserRoles.Contains(roleName))
        {
            userDetails.UserRoles.Remove(roleName);
        }

        var command = new UpdateUserRoleCommand()
        {
            Role = roleName,
            UserId = UserId
        };
        await Mediator.Send(command);
    }
}