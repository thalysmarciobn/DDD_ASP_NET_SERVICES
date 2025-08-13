namespace Auth.Application.Common;

public record RegisterUserData
{
    public Guid UserId { get; init; }
}

public record LoginUserData
{
    public string Token { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public record UserData
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; init; }
    public bool IsActive { get; init; }
}
