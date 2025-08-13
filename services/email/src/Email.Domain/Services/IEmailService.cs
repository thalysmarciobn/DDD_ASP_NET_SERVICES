namespace Email.Domain.Services;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string to, string username, string verificationCode, CancellationToken cancellationToken = default);
    Task<bool> SendResendVerificationEmailAsync(string to, string username, string verificationCode, CancellationToken cancellationToken = default);
}
