using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OliveBlazor.Core.Application.Services.Email;
using OliveBlazor.Infrastructure.Indentity;

namespace OliveBlazor.Infrastructure.Services;

public class Smtp2GoEmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserIdentity> _userManager;

    public Smtp2GoEmailService(string smtpServer, int port, string username, string password, IConfiguration configuration, UserManager<UserIdentity> userManager)
    {
        _smtpServer = smtpServer;
        _port = port;
        _username = username;
        _password = password;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailConfirmationEnabled = _configuration.GetValue<bool>("EmailConfirmation:Enabled");

        if (!emailConfirmationEnabled)
        {
            // If email confirmation is disabled, bypass sending email and directly confirm the email.
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);
            }
            return;
        }

        using (var smtpClient = new SmtpClient(_smtpServer, _port))
        {
            smtpClient.Credentials = new NetworkCredential(_username, _password);
            smtpClient.EnableSsl = true; // Ensure this is set if required by your SMTP server

            var mailMessage = new MailMessage("SammyHammami@my.uopeople.edu", email, subject, message);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}