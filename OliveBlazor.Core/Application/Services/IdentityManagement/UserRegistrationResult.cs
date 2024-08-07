namespace OliveBlazor.Core.Application.Services.IdentityManagement;

public class UserRegistrationResult
{
    public string UserId { get; set; }

    public string EmailConfirmationToken { get; set; }
}