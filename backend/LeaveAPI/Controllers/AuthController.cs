using LeaveAPI.Data;
using LeaveAPI.DTOs;
using LeaveAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(ApplicationDbContext dbContext, IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        var token = _jwtTokenService.BuildToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role?.Name ?? string.Empty
        });
    }
}
