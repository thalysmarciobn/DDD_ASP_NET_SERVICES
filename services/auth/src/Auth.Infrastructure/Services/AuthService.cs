using Auth.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Auth.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            if (hash.StartsWith("$2"))
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            else
            {
                var passwordHash = HashPasswordWithSHA256(password);
                return passwordHash == hash;
            }
        }
        catch
        {
            return false;
        }
    }

    private string HashPasswordWithSHA256(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public string GenerateJwtToken(string username, string email, Guid userId)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "super-secret-key-123456789-abcdefghijklmnopqrstuvwxyz-32-chars-minimum";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "auth-service";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "auth-service";
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
