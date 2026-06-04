namespace LeaveAPI.DTOs;

public class ApplyLeaveRequest
{
    public int LeaveTypeId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
