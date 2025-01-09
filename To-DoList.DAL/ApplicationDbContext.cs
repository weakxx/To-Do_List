using Microsoft.EntityFrameworkCore;
using To_DoList.Domain.Entity;

namespace To_DoList.DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<TaskEntity> Tasks { get; set; }
}