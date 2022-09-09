//using MailKit.Net.Smtp;
//using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using Shop.Models;

using System.Text.Json;

namespace Shop.Services
{
    public class EmailSender : ICustomEmailSender
    {
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            string from = string.Empty;
            string code_auth = string.Empty;
            using (StreamReader sr = new StreamReader("mailsender.txt"))
            {
                var file = JsonSerializer.Deserialize<maildata>(sr.ReadToEnd());
                from = file?.mail;
                code_auth = file?.code;
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("NEW ITEM", from));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };            

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("multishopannounce@gmail.com", code_auth);
                await client.SendAsync(emailMessage);               
                await client.DisconnectAsync(true);
            }
        }      
    }
}
