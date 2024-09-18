using Haiku.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Haiku.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HaikuAPIContext _context;

        public UserRepository(HaikuAPIContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }
    }

}
