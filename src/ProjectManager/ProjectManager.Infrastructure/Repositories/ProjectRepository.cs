using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private AppDbContext _db;

        public ProjectRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddProject(Project project)
        {
            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();
        }

        public Task Commit() => _db.SaveChangesAsync();

        public Task<Project?> GetProjectById(int id)
            =>  _db.Projects.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Project>?> GetAll()
            => await _db.Projects.ToListAsync();

        public async Task<bool> RemoveProject(Project project)
        {
            if (await _db.Projects.FirstOrDefaultAsync(p => p.Id == project.Id) is null)
                return false;
            
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveProjectById(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
                return false;

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<Project?> UpdateProject(Project project)
        {            
            if (project is null || !await _db.Projects.Where(x => x.Id == project.Id).AnyAsync())
                return null;

            _db.Projects.Update(project);

            await _db.SaveChangesAsync();

            return project;
        }
    }
}
