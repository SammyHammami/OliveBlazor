using Microsoft.AspNetCore.Components;
using OliveBlazor.Core.Domain.Accounts;

namespace OliveBlazor.UI.Pages.Admin.Roles;

public partial class RoleDetails
{
    [Parameter] public string RoleId { get; set; }
    public OliveRole Role { get; set; }

    protected override async Task OnInitializedAsync()
    {
       // Role = await IdentityService.ge(RoleId);
    }
}