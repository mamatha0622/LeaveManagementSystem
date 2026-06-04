namespace LeaveAPI.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public User? Employee { get; set; }
    public int LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ManagerComment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
