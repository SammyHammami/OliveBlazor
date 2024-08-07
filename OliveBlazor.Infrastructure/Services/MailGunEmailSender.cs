using OlivePatterns.Core.Application.Services.Email;
using RestSharp;
using RestSharp.Authenticators;

namespace OlivePatterns.Infrastructure.Services;

public class MailGunEmailSender : ICustomEmailSender
{

   
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        

    }
}