using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<User> addUser(User user);
        Task<User> GetUserById(int id);
        Task<User> login(User user);
        void updateUser(int id, User user);

    }
}