using AutoMapper;
using DTOs;
using Entities;
using FluentNHibernate.Automapping;
using Repository;
namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        //AutoMapper _mapper;
        private readonly IMapper _mapper;


        public UserService (IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;

        }
        public async Task<GetUserDTO> GetUserById(int id)
        {
            User? user = await _userRepository.GetUserById(id);
            if(user == null) 
                return null;
            GetUserDTO userDto = _mapper.Map<GetUserDTO>(user);
            return userDto;
        }
    
        public async Task<GetUserDTO> addUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            if ((await _passwordService.CheckPasswordStrength(user.Password)).Strength <= 2)
                return null;
            User user1 = await _userRepository.addUser(user);
            GetUserDTO userDTO = _mapper.Map<GetUserDTO>(user1);
            return userDTO;
        }
        public void updateUser(int id, UserDto user)
        {
            _userRepository.UpdateUser(id,user);

        }
        public async Task<GetUserDTO> login(LoginDTO loginDto)
        {
            User user3= await _userRepository.login(loginDto);
            GetUserDTO userDto = _mapper.Map<GetUserDTO>(user3);
            return userDto;
        }
    }
}
