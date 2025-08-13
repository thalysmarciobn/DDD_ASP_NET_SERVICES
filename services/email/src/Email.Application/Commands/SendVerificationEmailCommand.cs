using Common.CQRS;
using Email.Application.Common;

namespace Email.Application.Commands;

public record SendVerificationEmailCommand : ICommand<Result<SendVerificationEmailData>>
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
}
