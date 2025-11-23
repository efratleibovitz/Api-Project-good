using Entities;

namespace Services
{
    public interface IPasswordService
    {
        PasswordEntity Level(string password);
    }
}