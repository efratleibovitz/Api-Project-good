using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<GetUserDTO> addUser(UserDto user);
        Task<GetUserDTO> GetUserById(int id);
        Task<GetUserDTO> login(LoginDTO loginDto);
        void updateUser(int id, UserDto user);

    }
}