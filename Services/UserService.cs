using Entities;
using Repository;
namespace Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IPasswordService _passwordService;

        public UserService (IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }
        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        public async Task<User> addUser(User user)
        {
            if (_passwordService.Level(user.Password).Strength <= 2)
                return null;
            
            return await _userRepository.addUser(user);
        }
        public void updateUser(int id, User user)
        {
            _userRepository.updateUser(id, user);

        }
        public async Task<User> login(User user)
        {
            return await _userRepository.login(user);
        }
    }
}
