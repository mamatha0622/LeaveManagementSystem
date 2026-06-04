namespace LeaveAPI.Models;

public class LeaveType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
