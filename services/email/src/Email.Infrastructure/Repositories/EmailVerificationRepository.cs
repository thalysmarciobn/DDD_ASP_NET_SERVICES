using Email.Domain.Entities;
using Email.Domain.Repositories;
using Email.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Email.Infrastructure.Repositories;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly EmailDbContext _context;

    public EmailVerificationRepository(EmailDbContext context)
    {
        _context = context;
    }

    public async Task<EmailVerification?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.EmailVerifications
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
    }

    public async Task<EmailVerification?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.EmailVerifications
            .FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<EmailVerification?> GetByVerificationCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.EmailVerifications
            .FirstOrDefaultAsync(e => e.VerificationCode == code, cancellationToken);
    }

    public async Task<EmailVerification> CreateAsync(EmailVerification verification, CancellationToken cancellationToken = default)
    {
        _context.EmailVerifications.Add(verification);
        await _context.SaveChangesAsync(cancellationToken);
        return verification;
    }

    public async Task<EmailVerification> UpdateAsync(EmailVerification verification, CancellationToken cancellationToken = default)
    {
        _context.EmailVerifications.Update(verification);
        await _context.SaveChangesAsync(cancellationToken);
        return verification;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var verification = await _context.EmailVerifications.FindAsync(new object[] { id }, cancellationToken);
        if (verification == null)
            return false;

        _context.EmailVerifications.Remove(verification);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
