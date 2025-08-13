using Auth.Application.CQRS;
using Auth.Application.Common;

namespace Auth.Application.Queries;

public record GetUserQuery : IQuery<Result<UserData>>
{
    public Guid UserId { get; init; }
}
