using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMSProject.Models;

namespace PMSProject.Data
{
    public class AppDbContext : IdentityDbContext
    {
       
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<SprintModel> Sprints { get; set; }

        public DbSet<PublishHistoryModel> PublishHistory { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskModel>().HasQueryFilter(i => !i.IsDeleted);
            modelBuilder.Entity<ProjectModel>().HasQueryFilter(i => !i.IsDeleted);
            modelBuilder.Entity<SprintModel>().HasQueryFilter(i => !i.IsDeleted);
            modelBuilder.Entity<PublishHistoryModel>().HasQueryFilter(i => !i.IsDeleted);

            base.OnModelCreating(modelBuilder);
            // Clarifying the relationship between the parent project and the sub-project.
            modelBuilder.Entity<ProjectModel>()
                .HasOne(p => p.ParentProject)
                .WithMany(p => p.SubProjects)
                .HasForeignKey(p => p.ParentProjectID)
                .OnDelete(DeleteBehavior.Restrict); // use Restrict to avoid delete parent project has a sub projects
          }

    }
}
