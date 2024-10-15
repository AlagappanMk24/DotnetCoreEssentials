using JwtAuthDB.Entities;

namespace JwtAuthDB.Data.Repositories.Interface
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User GetUserByEmail(string email);
    }
}
