using LeaveAPI.Data;
using LeaveAPI.DTOs;
using LeaveAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeaveAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LeavesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public LeavesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaves([FromQuery] string? status)
    {
        var currentUserId = GetCurrentUserId();
        var currentRole = GetCurrentUserRole();

        var query = _dbContext.LeaveRequests
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .AsQueryable();

        if (currentRole == "Employee")
        {
            query = query.Where(l => l.EmployeeId == currentUserId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(l => l.Status == status);
        }

        var result = await query
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                l.Id,
                EmployeeName = l.Employee!.DisplayName ?? l.Employee.Username,
                LeaveType = l.LeaveType!.Name,
                l.FromDate,
                l.ToDate,
                l.Status,
                l.ManagerComment,
                l.CreatedAt
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveRequest request)
    {
        var currentUserId = GetCurrentUserId();
        var leaveType = await _dbContext.LeaveTypes.FindAsync(request.LeaveTypeId);

        if (leaveType == null)
        {
            return BadRequest(new { Message = "Leave type not found." });
        }

        var leave = new LeaveRequest
        {
            EmployeeId = currentUserId,
            LeaveTypeId = request.LeaveTypeId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.LeaveRequests.Add(leave);
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "Leave request submitted successfully." });
    }

    [AllowAnonymous]
    [HttpGet("types")]
    public async Task<IActionResult> GetLeaveTypes()
    {
        var types = await _dbContext.LeaveTypes
            .Select(t => new { t.Id, t.Name })
            .ToListAsync();

        return Ok(types);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ApproveLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null)
        {
            return NotFound();
        }

        leave.Status = "Approved";
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "Leave request approved." });
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> RejectLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        if (leave == null)
        {
            return NotFound();
        }

        leave.Status = "Rejected";
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "Leave request rejected." });
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private string GetCurrentUserRole()
    {
        return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}
