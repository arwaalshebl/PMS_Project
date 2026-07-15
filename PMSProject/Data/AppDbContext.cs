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

    }
}
