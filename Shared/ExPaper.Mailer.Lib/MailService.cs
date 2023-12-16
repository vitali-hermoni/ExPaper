using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace ExPaper.Mailer.Lib;

public class MailService
{
    public static async void SendEmailAsync(EmailDto request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("expaper@nobicore.de"));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smtp = new SmtpClient();
        smtp.Connect("nobicore.de", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("expaper@nobicore.de", "!ExPaper_0815_!ExPaper_0815");
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
