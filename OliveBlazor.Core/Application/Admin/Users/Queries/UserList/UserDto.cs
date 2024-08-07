namespace OlivePatterns.Core.Application.Admin.Users.Queries.UserList;

public class UserDto
{
    public string Id { get; }
    public string UserName { get; }
    public string Email { get; }
    public bool EmailConfirmed { get; set; }
    public UserDto(string uId, string uUserName, string uEmail, bool emailConfirmed)
    {
        Id = uId;
        UserName = uUserName;
        Email = uEmail;
        EmailConfirmed = emailConfirmed;
    }
}