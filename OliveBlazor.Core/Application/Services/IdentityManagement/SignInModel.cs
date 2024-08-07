namespace OliveBlazor.Core.Application.Services.IdentityManagement;

public class SignInModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
public class SignInResponse
{
    public bool Succeeded { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public bool IsLockedOut { get; set; }
    public bool IsNotAllowed { get; set; } // Additional case
   
}