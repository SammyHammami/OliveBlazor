using Microsoft.AspNetCore.Components;
using OliveBlazor.Core.Application.Admin.Users.Queries.Common;

namespace OliveBlazor.UI.Pages.Admin.Users.Components;

public partial class UsersList
{
    [Parameter]
    public List<UserDto> Users { get; set; }

    [Parameter]
    public bool CanManageUsers { get; set; }

    protected override async Task OnInitializedAsync()
    {


    }




}



