using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class ProjectTask
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public Project Project { get; set; }

        public int? AssignedTo { get; set; }
        
        public User AssignedUser { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public ProjectTaskStatus Status { get; set; } 
        
        public ProjectTaskPriority Priority { get; set; } 
        
        public DateTime? DueDate { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public ICollection<TaskComment> TaskComments { get; set; }
    }
}
