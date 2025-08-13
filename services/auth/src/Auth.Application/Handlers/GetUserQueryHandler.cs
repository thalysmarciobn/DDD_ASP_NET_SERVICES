using Common.CQRS;
using Auth.Application.Common;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Application.Queries;

namespace Auth.Application.Handlers;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, Result<UserData>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserData>> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            if (user == null)
            {
                return Result<UserData>.Error((int)AuthErrorCode.UserNotFound);
            }

            var resultData = new UserData
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive
            };

            return Result<UserData>.Success(resultData, (int)SuccessCode.UserRetrieved);
        }
        catch (Exception)
        {
            return Result<UserData>.Error((int)AuthErrorCode.DatabaseError);
        }
    }
}
