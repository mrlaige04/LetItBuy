namespace Shop.Services
{
    public interface ICustomEmailSender
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
    }
}
