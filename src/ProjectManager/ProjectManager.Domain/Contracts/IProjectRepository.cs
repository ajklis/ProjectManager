using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Contracts
{
    public interface IProjectRepository
    {
        Task Commit();
        Task<Project?> GetProjectById(int id);
        Task<IEnumerable<Project>?> GetAll();
        Task AddProject(Project project);
        Task<bool> RemoveProject(Project project);
        Task<bool> RemoveProjectById(int id);
        Task<Project?> UpdateProject(Project project);
    }
}
