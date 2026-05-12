using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisDatabase;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IPasswordService passwordService,
            IMapper mapper, IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
            _redisDatabase = redis.GetDatabase();
            _configuration = configuration;
        }

        public async Task<GetUserDTO> GetUserById(int id)
        {
            string key = $"user:{id}";
            var cachedUser = await _redisDatabase.StringGetAsync(key);
            if (cachedUser.HasValue)
                return JsonSerializer.Deserialize<GetUserDTO>(cachedUser)!;

            User? user = await _userRepository.GetUserById(id);
            if (user == null) return null!;

            GetUserDTO userDto = _mapper.Map<GetUserDTO>(user);
            await _redisDatabase.StringSetAsync(key, JsonSerializer.Serialize(userDto), TimeSpan.FromMinutes(5));
            return userDto;
        }

        public async Task<string?> addUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            if ((await _passwordService.CheckPasswordStrength(user.Password)).Strength <= 2)
                return null;
            User created = await _userRepository.addUser(user);
            return GenerateToken(created);
        }

        public void updateUser(int id, UserDto user)
        {
            _userRepository.UpdateUser(id, user);
            _redisDatabase.KeyDelete($"user:{id}");
        }

        public async Task<string?> login(LoginDTO loginDto)
        {
            User? user = await _userRepository.login(loginDto);
            if (user == null) return null;
            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                new Claim(ClaimTypes.Role, user.UserEmail.StartsWith("admin") ? "Admin" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(u => _mapper.Map<GetUserDTO>(u));
        }

    }
}
