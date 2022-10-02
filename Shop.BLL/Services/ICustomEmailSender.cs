using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
    public interface ICustomEmailSender
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
    }
}
