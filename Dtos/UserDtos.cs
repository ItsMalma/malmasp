namespace Malmasp.Dtos;

public class UserResponseDto
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserRequestDto
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}