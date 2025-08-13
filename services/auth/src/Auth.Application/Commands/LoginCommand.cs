using Auth.Application.CQRS;
using Auth.Application.Common;

namespace Auth.Application.Commands;

public record LoginCommand : ICommand<Result<LoginUserData>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
