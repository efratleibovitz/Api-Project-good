using DTOs;
using Entities;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<User?> GetUserById(int id);
        Task<User?> login(LoginDTO loginDto);
        Task UpdateUser(int id, UserDto userDto);
    }
}