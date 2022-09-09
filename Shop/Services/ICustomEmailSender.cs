namespace Shop.Services
{
    public interface ICustomEmailSender
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
}
