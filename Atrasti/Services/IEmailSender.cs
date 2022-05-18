using System.Threading.Tasks;

namespace Atrasti.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}
