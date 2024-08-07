using Microsoft.AspNetCore.Identity.UI.Services;

namespace OlivePatterns.Infrastructure.Services;

public class EmailService : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Send Emails 
    }
}