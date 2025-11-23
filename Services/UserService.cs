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
        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }
        public User addUser(User user)
        {
            if (_passwordService.Level(user.passWord).Strength <= 2)
                return null;
            
            return _userRepository.addUser(user);
        }
        public void updateUser(int id, User user)
        {
            _userRepository.updateUser(id, user);

        }
        public User login(User user)
        {
            return _userRepository.login(user);
        }
    }
}
