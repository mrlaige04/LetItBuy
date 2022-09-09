//using MailKit.Net.Smtp;
//using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using Shop.Models;

using System.Text.Json;

namespace Shop.Services
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
        public async Task SendEmailAsync(string to, string subject, string message)
        {           
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("LetItBuy", _mail));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };            

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(_mail, _code);
                await client.SendAsync(emailMessage);               
                await client.DisconnectAsync(true);
            }
        }      
    }
}
