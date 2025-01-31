using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Contracts
{
    public interface IProjectTaskRepository
    {
        Task Commit();
        Task<IEnumerable<ProjectTask>?> GetTasksForProject(Project project);
        Task<IEnumerable<ProjectTask>?> GetTasksForProject(int projectId);
        Task<IEnumerable<ProjectTask>?> GetTasksForUser(User user);
        Task<IEnumerable<ProjectTask>?> GetTasksForUser(int userId);
        Task AddTask(int projectId, ProjectTask task);
        Task<bool> RemoveTask(ProjectTask task);
        Task<bool> RemoveTaskById(int taskId);
        Task<ProjectTask?> Update(ProjectTask task);
    }
}
