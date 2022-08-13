using System.Threading.Tasks;
using Traffic.Application.Models.SendEmail;

namespace Traffic.Application.Interfaces
{
    public interface IEmailService
    {
        Task<SendMailModel> SendMail();
    }
}
