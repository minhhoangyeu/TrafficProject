using SendGrid;
using System.Threading.Tasks;
using Traffic.Application.Models.SendEmail;

namespace Traffic.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}
