namespace OliveBlazor.Core.Application.Services.IdentityManagement;

public class UserRegistrationDto
{

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string Password {
        get;
        set;
    }

    public string UserName { get; set; }
}