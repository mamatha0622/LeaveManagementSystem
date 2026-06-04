using LeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Employee" },
            new Role { Id = 2, Name = "Manager" },
            new Role { Id = 3, Name = "Admin" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "employee", Password = "password", RoleId = 1, DisplayName = "Employee User" },
            new User { Id = 2, Username = "manager", Password = "password", RoleId = 2, DisplayName = "Manager User" },
            new User { Id = 3, Username = "admin", Password = "password", RoleId = 3, DisplayName = "Administrator" }
        );

        modelBuilder.Entity<LeaveType>().HasData(
            new LeaveType { Id = 1, Name = "Sick Leave" },
            new LeaveType { Id = 2, Name = "Vacation" },
            new LeaveType { Id = 3, Name = "Personal" }
        );

        modelBuilder.Entity<LeaveRequest>()
            .Property(l => l.Status)
            .HasDefaultValue("Pending");
    }
}
