using ProjectManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public string? Description { get; set; }

        public ProjectTaskStatus Status { get; set; }

        public ProjectTaskPriority Priority { get; set; }

        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }

        public ICollection<ProjectTask> ProjectTasks { get; set; }

        public Project(int id, string name, string? description, ProjectTaskStatus status, ProjectTaskPriority priority, DateTime? startDate, DateTime? endDate)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Project() { }
    }
}
