using JwtAuthDB.Data.Context;
using JwtAuthDB.Data.Context;
using JwtAuthDB.Data.Repositories.Interface;
using JwtAuthDB.Entities;

namespace JwtAuthDB.Data.Repositories
{
    public class UserRepository(JwtContext context) : IUserRepository
    {
        private readonly JwtContext _context = context;
        public User AddUser(User user)
        {
            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }
        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }
    }
}
