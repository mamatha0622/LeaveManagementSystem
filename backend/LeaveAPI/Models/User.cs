namespace LeaveAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? DisplayName { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
