using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;
using OliveBlazor.Core.Application.Admin.Roles.Commands.RemoveRole;
using OliveBlazor.Core.Application.Admin.Roles.Queries.GetRoles;

namespace OliveBlazor.UI.Pages.Admin.Roles;

public partial class Index
{

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public required IMediator Mediator { get; set; }

    public RolesVm? Model { get; set; }

    private string newRoleName = string.Empty;
    private Guid  roleIdToDelete;
    protected override async Task OnInitializedAsync()
    {
        var query = new GetRolesQuery();
        Model = await Mediator.Send(query);

    }



    private async Task AddRole()
    {
        if (!string.IsNullOrWhiteSpace(newRoleName))
        {
            var addRoleCommand = new AddRoleCommand()
            {
                Name = newRoleName
            };

            var dto  = await Mediator.Send(addRoleCommand);

            Model!.Roles.Add(dto);
        }

        newRoleName = string.Empty;
    }


   
    private async Task RemoveRole(Guid roleId)
    {
        bool isConfirmed = await JSRuntime.InvokeAsync<bool>("confirmDelete", "Are you sure you want to delete this role?");
        if (isConfirmed)
        {
            var removeRoleCommand = new RemoveRoleCommand()
            {
                Id = roleId
            };

            try
            {
                await Mediator.Send(removeRoleCommand);

                var roleToRemove = Model!.Roles.FirstOrDefault(r => r.Id == roleId);
                if (roleToRemove != null)
                {
                    Model.Roles.Remove(roleToRemove);
                }
            }
            catch (InvalidOperationException ex)
            {

                Console.WriteLine($"Failed to remove role: {ex.Message}");

            }
        }
    }
}