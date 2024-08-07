using OliveBlazor.Core.Application.Admin.Users.Queries.Common;

namespace OliveBlazor.Core.Application.Admin.Users.Queries.UserList;

public class UsersVm
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}