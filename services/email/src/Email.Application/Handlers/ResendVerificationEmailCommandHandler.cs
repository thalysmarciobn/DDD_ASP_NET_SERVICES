using Common.CQRS;
using Common.Utilities.CodeGeneration;
using Email.Application.Common;
using Email.Application.Commands;
using Email.Domain.Entities;
using Email.Domain.Repositories;
using Email.Domain.Services;

namespace Email.Application.Handlers;

public class ResendVerificationEmailCommandHandler : ICommandHandler<ResendVerificationEmailCommand, Result<ResendVerificationEmailData>>
{
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IEmailService _emailService;

    public ResendVerificationEmailCommandHandler(
        IEmailVerificationRepository emailVerificationRepository,
        IEmailService emailService)
    {
        _emailVerificationRepository = emailVerificationRepository;
        _emailService = emailService;
    }

    public async Task<Result<ResendVerificationEmailData>> HandleAsync(ResendVerificationEmailCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingVerification = await _emailVerificationRepository.GetByUserIdAsync(command.UserId, cancellationToken);
            if (existingVerification == null)
            {
                return Result<ResendVerificationEmailData>.Error((int)EmailErrorCode.EmailNotFound);
            }

            if (existingVerification.IsVerified)
            {
                return Result<ResendVerificationEmailData>.Error((int)EmailErrorCode.EmailAlreadyVerified);
            }

            if (existingVerification.Attempts >= 5)
            {
                return Result<ResendVerificationEmailData>.Error((int)EmailErrorCode.MaxAttemptsExceeded);
            }

            var verificationCode = VerificationCodeGenerator.GenerateSecureCode(6);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            existingVerification.VerificationCode = verificationCode;
            existingVerification.ExpiresAt = expiresAt;
            existingVerification.Attempts++;

            await _emailVerificationRepository.UpdateAsync(existingVerification, cancellationToken);

            var emailSent = await _emailService.SendVerificationEmailAsync(
                command.Email,
                command.Username,
                verificationCode,
                cancellationToken);

            if (!emailSent)
            {
                return Result<ResendVerificationEmailData>.Error((int)EmailErrorCode.EmailSendFailed);
            }

            var resultData = new ResendVerificationEmailData
            {
                VerificationId = existingVerification.Id,
                ExpiresAt = expiresAt,
                Attempts = existingVerification.Attempts
            };

            return Result<ResendVerificationEmailData>.Success(resultData, (int)EmailSuccessCode.VerificationEmailResent);
        }
        catch (Exception)
        {
            return Result<ResendVerificationEmailData>.Error((int)EmailErrorCode.DatabaseError);
        }
    }
}
