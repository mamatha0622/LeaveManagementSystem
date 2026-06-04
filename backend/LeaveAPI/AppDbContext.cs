using LeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<LeaveRequest> LeaveRequests { get; set; }
}
