using System.Threading.Tasks;
using API.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        public async Task sendMailAsync(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("datthenaythiunique@gmail.com");
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = html;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("datthenaythiunique@gmail.com", "123123MMm@");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
