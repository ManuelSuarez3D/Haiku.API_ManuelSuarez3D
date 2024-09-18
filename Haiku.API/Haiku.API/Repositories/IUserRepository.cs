using Haiku.API.Models;

namespace Haiku.API.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
    }
}
