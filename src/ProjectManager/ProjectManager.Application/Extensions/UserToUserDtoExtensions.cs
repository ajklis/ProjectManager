using ProjectManager.Application.Models;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Extensions
{
    public static class UserToUserDtoExtensions
    {
        public static List<UserDto> ToUserDtoList(this IEnumerable<User> users) => users.Select(u => UserDto.FromUser(u)).ToList();
        //public static List<UserDto> ToUserDtoList(this IEnumerable<User> users) => users.ToUserDtoList();
    }
}
