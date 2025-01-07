using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        
        public AppDbContext(IConfiguration config, DbContextOptions options) : base(options)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(_config.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(_config.GetConnectionString("DefaultConnection")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // enum handeling
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>();
            modelBuilder.Entity<UserProject>()
                .Property(up => up.RoleInProject)
                .HasConversion<string>();

            // id handeling
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ProjectTask>()
                .Property(pt => pt.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(pt => pt.AssignedTo)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.ProjectTask)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany()
                .HasForeignKey(tc => tc.UserId);
        }

    }
}
