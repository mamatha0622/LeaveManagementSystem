using LeaveAPI.Data;
using LeaveAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users
            .Include(u => u.Role)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                DisplayName = u.DisplayName ?? u.Username,
                Role = u.Role!.Name
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _dbContext.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
            .ToListAsync();

        return Ok(roles);
    }

    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleRequest request)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        var role = await _dbContext.Roles.FindAsync(request.RoleId);
        if (role == null)
        {
            return BadRequest(new { Message = "Invalid role." });
        }

        user.RoleId = request.RoleId;
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "User role updated successfully." });
    }

    [HttpGet("leavetypes")]
    public async Task<IActionResult> GetLeaveTypes()
    {
        var types = await _dbContext.LeaveTypes
            .Select(t => new { t.Id, t.Name })
            .ToListAsync();

        return Ok(types);
    }
}
