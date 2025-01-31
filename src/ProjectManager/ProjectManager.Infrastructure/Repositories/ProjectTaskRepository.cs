using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly AppDbContext _db;
        
        public ProjectTaskRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task Commit()
            => _db.SaveChangesAsync();

        public async Task AddTask(int projectId, ProjectTask task)
        {
            if (!await _db.Projects.Where(x => x.Id == projectId).AnyAsync())
                return;

            await _db.ProjectTasks.AddAsync(task);
            await _db.SaveChangesAsync();
        }

        public Task<IEnumerable<ProjectTask>?> GetTasksForProject(Project project)
            => GetTasksForProject(project.Id);

        public async Task<IEnumerable<ProjectTask>?> GetTasksForProject(int projectId)
        {
            var project = await _db.Projects.Where(x => x.Id == projectId).FirstAsync();

            if (project is null)
                return null;

            return await _db.ProjectTasks.Where(x => x.ProjectId == project.Id).ToListAsync();
        }

        public Task<IEnumerable<ProjectTask>?> GetTasksForUser(User user)
            => GetTasksForUser(user.Id);

        public async Task<IEnumerable<ProjectTask>?> GetTasksForUser(int userId)
        {
            var user = await _db.Users.Where(x => x.Id == userId).FirstAsync();

            if (user is null)
                return null;

            return await _db.ProjectTasks.Where(x => x.AssignedUser.Id == userId).ToListAsync();
        }

        public async Task<bool> RemoveTask(ProjectTask task)
        {
            if (!await _db.ProjectTasks.ContainsAsync(task))
                return false;

            _db.ProjectTasks.Remove(task);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTaskById(int taskId)
        {
            var task = await _db.ProjectTasks.Where(x => x.Id == taskId).FirstAsync();

            if (task is null)
                return false;

            return await RemoveTask(task);
        }

        public async Task<ProjectTask?> Update(ProjectTask task)
        {
            if (task is null || !await _db.ProjectTasks.Where(x => x.Id == task.Id).AnyAsync())
                return null;

            _db.ProjectTasks.Update(task);
            await _db.SaveChangesAsync();
            return task;
        }
    }
}
