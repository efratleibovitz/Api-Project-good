using Entities;

namespace Services
{
    public interface IUserService
    {
        User addUser(User user);
        User GetUserById(int id);
        User login(User user);
        void updateUser(int id, User user);
    }
}