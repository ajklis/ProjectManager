using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Contracts
{
    public interface IUserRepository
    {
        Task Commit();
        Task AddUser(User user);
        Task<bool> RemoveUser(User user);
        Task<bool> RemoveUserById(int id);
        Task<User?> GetUserById(int id);
        Task<IEnumerable<User>?> GetAll();
        Task<User?> UpdateUser(User user);
    }
}
