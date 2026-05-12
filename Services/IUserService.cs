using DTOs;

namespace Services
{
    public interface IUserService
    {
        Task<string?> addUser(UserDto user);
        Task<GetUserDTO> GetUserById(int id);
        Task<string?> login(LoginDTO loginDto);
        void updateUser(int id, UserDto user);
        Task<IEnumerable<GetUserDTO>> GetAllUsersAsync();

    }
}
