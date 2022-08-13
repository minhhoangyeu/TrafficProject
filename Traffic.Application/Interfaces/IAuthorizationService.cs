using Traffic.Application.Dtos;
using System.Threading.Tasks;

namespace Traffic.Application.Interfaces
{
    public interface IAuthorizationService
    {
        Task<UserDto> GetUserAsync(string token);
    }
}
