namespace LeaveAPI.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Role { get; set; } = null!;
}
