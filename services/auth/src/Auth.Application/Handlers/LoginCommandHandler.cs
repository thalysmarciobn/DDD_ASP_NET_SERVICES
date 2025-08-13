using Auth.Application.Commands;
using Auth.Application.CQRS;
using Auth.Application.Common;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Handlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginUserData>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Result<LoginUserData>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username);
        if (user == null)
        {
            return Result<LoginUserData>.Error(AuthErrorCode.UserNotFound);
        }

        if (!user.IsActive)
        {
            return Result<LoginUserData>.Error(AuthErrorCode.UserInactive);
        }

        if (!_authService.VerifyPassword(command.Password, user.PasswordHash))
        {
            return Result<LoginUserData>.Error(AuthErrorCode.InvalidCredentials);
        }

        var token = _authService.GenerateJwtToken(user.Username, user.Email, user.Id);

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var data = new LoginUserData
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };

        return Result<LoginUserData>.Success(data, SuccessCode.UserLoggedIn);
    }
}
