using OlivePatterns.Core.Application.Services.Email;
using System.Net.Mail;
using System.Net;

namespace OlivePatterns.Infrastructure.Services;

public class TestMailEmailSender : ICustomEmailSender
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public TestMailEmailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true
        };

        return client.SendMailAsync(_smtpUsername, email, subject, htmlMessage);
    }
}
