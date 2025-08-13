namespace Auth.Domain.Services;

public interface IAuthService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    string GenerateJwtToken(string username, string email, Guid userId);
}
