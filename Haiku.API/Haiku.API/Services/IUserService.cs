using Haiku.API.Models;

namespace Haiku.API.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
