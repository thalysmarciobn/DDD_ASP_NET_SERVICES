using Common.CQRS;
using Auth.Application.Common;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Auth.Infrastructure.Services;
using Auth.Application.Commands;

namespace Auth.Application.Handlers;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<RegisterUserData>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IMessageQueueService _messageQueueService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository, 
        IAuthService authService,
        IMessageQueueService messageQueueService)
    {
        _userRepository = userRepository;
        _authService = authService;
        _messageQueueService = messageQueueService;
    }

    public async Task<Result<RegisterUserData>> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            {
                return Result<RegisterUserData>.Error((int)AuthErrorCode.InvalidInput);
            }

            var existingUser = await _userRepository.GetByUsernameAsync(command.Username);
            if (existingUser != null)
            {
                return Result<RegisterUserData>.Error((int)AuthErrorCode.UserAlreadyExists);
            }

            existingUser = await _userRepository.GetByEmailAsync(command.Email);
            if (existingUser != null)
            {
                return Result<RegisterUserData>.Error((int)AuthErrorCode.UserAlreadyExists);
            }

            var hashedPassword = _authService.HashPassword(command.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Email = command.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            _messageQueueService.PublishUserCreated(user.Id, user.Email, user.Username);

            var resultData = new RegisterUserData
            {
                UserId = user.Id
            };

            return Result<RegisterUserData>.Success(resultData, (int)SuccessCode.UserRegistered);
        }
        catch (Exception)
        {
            return Result<RegisterUserData>.Error((int)AuthErrorCode.DatabaseError);
        }
    }
}
