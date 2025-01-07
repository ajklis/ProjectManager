namespace ProjectManager.Domain.Entities
{
    public class TaskComment
    {
        public int Id { get; set; }

        public int ProjectTaskId { get; set; }

        public int TaskId { get; set; }
        
        public ProjectTask ProjectTask { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }

        public string Comment { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
