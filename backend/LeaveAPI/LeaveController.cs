using LeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeaveController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.LeaveRequests.ToList());
    }

    [HttpPost]
    public IActionResult ApplyLeave(LeaveRequest request)
    {
        request.Status = "Pending";
        _context.LeaveRequests.Add(request);
        _context.SaveChanges();
        return Ok(request);
    }
}