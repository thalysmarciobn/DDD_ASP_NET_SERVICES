using Common.CQRS;
using Auth.Application.Commands;
using Auth.Application.Queries;
using Auth.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IDispatcher dispatcher, ILogger<AuthController> logger)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _dispatcher.SendAsync<RegisterUserCommand, Result<RegisterUserData>>(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            });
        }

        return Ok(new
        {
            SuccessCode = result.SuccessCode,
            Data = result.Data
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _dispatcher.SendAsync<LoginCommand, Result<LoginUserData>>(command);
        
        if (!result.IsSuccess)
        {
            return Unauthorized(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            });
        }

        _logger.LogInformation("User {Username} logged in successfully", command.Username);
        return Ok(new
        {
            SuccessCode = result.SuccessCode,
            Data = result.Data
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.LogWarning("Invalid or missing NameIdentifier claim");
            return Unauthorized(new
            {
                ErrorCode = AuthErrorCode.InvalidCredentials,
                ErrorMessage = "Invalid token - missing or invalid user ID"
            });
        }

        var query = new GetUserQuery { UserId = userId };
        var result = await _dispatcher.QueryAsync<GetUserQuery, Result<UserData>>(query);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to retrieve user data for ID {UserId}: {ErrorCode}", userId, result.ErrorCode);
            return NotFound(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            });
        }

        return Ok(new
        {
            SuccessCode = result.SuccessCode,
            Data = result.Data
        });
    }
}
