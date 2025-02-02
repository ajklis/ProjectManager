using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public ProjectTaskStatus Status { get; set; }

        public ProjectTaskPriority Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        private ProjectDto(int id, string name, string? description, ProjectTaskStatus status, ProjectTaskPriority priority, DateTime? startDate, DateTime? endDate, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = createdAt;
        }

        public static ProjectDto FromProject(Project project)
            => new ProjectDto(project.Id, project.Name, project.Description, project.Status, project.Priority, project.StartDate, project.EndDate, project.CreatedAt);
    }
}
