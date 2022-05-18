using System.Threading.Tasks;

namespace Atrasti.API.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}