using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task Commit() => _db.SaveChangesAsync();

        public async Task AddUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>?> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<bool> RemoveUser(User user)
        {
            if (!await _db.Users.Where(x => x.Id == user.Id).AnyAsync()) // user does not exist
                return false;
            
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveUserById(int id)
        {
            var user = await GetUserById(id);

            if (user is null)
                return false;
            
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            
            return true;
        }

        public async Task<User?> UpdateUser(User user)
        {
            if (user is null || !await _db.Users.Where(x => x.Id == user.Id).AnyAsync())
                return null;

            _db.Users.Update(user);

            await _db.SaveChangesAsync();

            return user;
        }
    }
}
