using Email.Domain.Entities;

namespace Email.Domain.Repositories;

public interface IEmailVerificationRepository
{
    Task<EmailVerification?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<EmailVerification?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<EmailVerification?> GetByVerificationCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<EmailVerification> CreateAsync(EmailVerification verification, CancellationToken cancellationToken = default);
    Task<EmailVerification> UpdateAsync(EmailVerification verification, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
