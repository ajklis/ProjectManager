using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class UserProject
    {
        public int UserId { get; set; }
        
        public User User { get; set; }

        public int ProjectId { get; set; }
        
        public Project Project { get; set; }

        public ProjectUserRole RoleInProject { get; set; }
    }
}
