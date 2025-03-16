using ProjectManager.Domain.Enums;

namespace ProjectManager.API.Models
{
    public class UserAddModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
