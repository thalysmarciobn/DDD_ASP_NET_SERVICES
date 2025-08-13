using Auth.Application.Commands;
using Auth.Application.CQRS;
using Auth.Application.Common;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Handlers;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<RegisterUserData>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Result<RegisterUserData>> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.ExistsAsync(command.Username, command.Email);
        if (exists)
        {
            return Result<RegisterUserData>.Error(AuthErrorCode.UserAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Email = command.Email,
            PasswordHash = _authService.HashPassword(command.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await _userRepository.AddAsync(user);

        var data = new RegisterUserData
        {
            UserId = createdUser.Id
        };

        return Result<RegisterUserData>.Success(data, SuccessCode.UserRegistered);
    }
}
