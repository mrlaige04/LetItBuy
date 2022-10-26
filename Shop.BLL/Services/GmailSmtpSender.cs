using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Shop.BLL.Services
{
    public class GmailSmtpSender : ICustomEmailSender
    {
        private readonly string _mail;
        private readonly string _code;
        public GmailSmtpSender(IConfiguration configuration)
        {
            _mail = configuration["emailhost:email"];
            _code = configuration["emailhost:code"];
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string message)
        {
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;

            var emailMessage = new MimeMessage();
            emailMessage.Body = bodyBuilder.ToMessageBody();
            emailMessage.From.Add(new MailboxAddress("LetItBuy", _mail));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync(_mail, _code);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch { return false; }
            }
        }
    }
}
