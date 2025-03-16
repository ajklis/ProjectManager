using ProjectManager.Domain.Contracts;
using System.Text;

namespace ProjectManager.Infrastructure.Services
{
    public sealed class PasswordHashingService : IPasswordHashingService
    {
        public string GetHashedPassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
    }
}
