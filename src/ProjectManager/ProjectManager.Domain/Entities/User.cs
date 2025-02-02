using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public UserRole? Role { get; set; } 
        
        public DateTime CreatedAt { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }
        
        public ICollection<ProjectTask> AssignedTasks { get; set; }
    }
}
