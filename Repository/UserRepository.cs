using System.Reflection.Metadata;
using System.Text.Json;
using DTOs;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebApiShop216328971Context _shopContext;
        public UserRepository(WebApiShop216328971Context userRepository)
        {
            _shopContext = userRepository;
        }
        public async Task<User?> GetUserById(int id)
        {
            return await _shopContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> addUser(User? user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();
            return user;
        }


        //public async Task<User?> UpdateUser(User user)
        //{
        //    var existingUser = await _shopContext.Users
        //        .FirstOrDefaultAsync(u => u.Id == user.Id);

        //    if (existingUser == null)
        //        return null;

        //    existingUser.FirstName = user.FirstName;
        //    existingUser.LastName = user.LastName;
        //    existingUser.UserEmail = user.UserEmail;

        //    await _shopContext.SaveChangesAsync();
        //    return existingUser;
        //}
        public async Task UpdateUser(int id, UserDto userDto)
        {
            //_webApiShopContext.Users.Update(user);
            User? user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return;

            user.UserEmail = userDto.UserName;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Password = userDto.Password;

            await _shopContext.SaveChangesAsync();
        }
        public async Task<User?> login(LoginDTO loginDto)
        {
             return await _shopContext.Users.FirstOrDefaultAsync(x => x.UserEmail == loginDto.UserEmail && x.Password == loginDto.Password);     

        }
    }
}
