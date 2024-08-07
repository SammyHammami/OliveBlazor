namespace OliveBlazor.Core.Application.Admin.Users.Queries.Common;

public class UserDto
{
    public string Id { get; }
    public string UserName { get; }
    public string Email { get; }
    public bool EmailConfirmed { get; set; }

    public List<string> Roles { get; set; }=new List<string>();
    public UserDto(string uId, string uUserName, string uEmail, bool emailConfirmed)
    {
        Id = uId;
        UserName = uUserName;
        Email = uEmail;
        EmailConfirmed = emailConfirmed;
    }
}