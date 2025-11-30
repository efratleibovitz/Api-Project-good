using System.Reflection.Metadata;
using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
    public class UserRepository : IUserRepository
    {
        WebApiShop216328971Context _shopContext;
        public UserRepository(WebApiShop216328971Context userRepository)
        {
            _shopContext = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _shopContext.Users.FindAsync(id);
        }

        public async Task<User> addUser(User user)
        {
            await _shopContext.Users.AddAsync(user);
            _shopContext.SaveChanges();
            return user;
        }

        public void updateUser(int id,User user)
        {
            _shopContext.Users.Update(user);
            _shopContext.SaveChanges();
    
        }
        public void DeleteUser(int id)
        {
          //  _shopContext.Users.Delete(id);
        }

        public async Task<User> login(User user)
        {
             return await _shopContext.Users.FirstOrDefaultAsync(x => x.UserEmail == user.UserEmail && x.Password == user.Password);     

        }
    }
}
