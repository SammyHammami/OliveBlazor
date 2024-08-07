using MediatR;
using Microsoft.AspNetCore.Components;
using OliveBlazor.Core.Application.Admin.Permission.Commands.SaveMatrix;
using OliveBlazor.Core.Application.Admin.Permission.Queries.GetPermissionMatrix;
using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.UI.Pages.Admin;

public partial class PermissionMatrix
{
    [Inject]
    public required IMediator Mediator { get; set; }


    public PermissionsMatrixViewModel Vm { get; set; } = new PermissionsMatrixViewModel();
    protected override async Task OnInitializedAsync()
    {
        var query = new GetPermissionMatrixQuery();
        Vm = await Mediator.Send(query);


    }
    private void TogglePermission(Permissions permission, RoleDto role)
    {
        if (Vm.Matrix[permission].Contains(role))
        {
            Vm.Matrix[permission].Remove(role);
            role.Permissions &= ~permission; // Remove the permission from the role
        }
        else
        {
            Vm.Matrix[permission].Add(role);
            role.Permissions |= permission; // Add the permission to the role
        }
    }

    private async void SaveMatrix()
    {
        var command = new SaveMatrixCommand()
        {
            Roles = Vm.Roles

        };
        await Mediator.Send(command);
    }

}