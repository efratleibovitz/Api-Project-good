using Entities;

namespace Services
{
    public interface IPasswordService
    {
        Task<PasswordEntity> CheckPasswordStrength(string password);
    }
}