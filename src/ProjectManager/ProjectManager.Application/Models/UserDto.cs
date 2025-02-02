using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.Models
{
    public record UserDto(int Id, string Name, string Email, UserRole? Role)
    {
        public static UserDto FromUser(User user) => new UserDto(user.Id, user.Name, user.Email, user.Role);
    }
}
