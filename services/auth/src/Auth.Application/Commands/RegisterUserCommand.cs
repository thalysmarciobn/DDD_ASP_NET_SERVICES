using Auth.Application.CQRS;
using Auth.Application.Common;

namespace Auth.Application.Commands;

public record RegisterUserCommand : ICommand<Result<RegisterUserData>>
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
