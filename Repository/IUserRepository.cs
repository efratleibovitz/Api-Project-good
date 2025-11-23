using Entities;

namespace Repository
{
    public interface IUserRepository
    {
        User addUser(User user);
        User GetUserById(int id);
        User login(User user);
        void updateUser(int id, User user);
    }
}