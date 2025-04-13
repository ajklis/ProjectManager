using ProjectManager.Domain.Enums;

namespace ProjectManager.API.Models
{
    public class UserRegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
