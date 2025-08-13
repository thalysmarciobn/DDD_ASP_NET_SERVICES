using Common.CQRS;
using Common.Utilities.CodeGeneration;
using Email.Application.Common;
using Email.Domain.Entities;
using Email.Domain.Repositories;
using Email.Domain.Services;
using Email.Application.Commands;

namespace Email.Application.Handlers;

public class SendVerificationEmailCommandHandler : ICommandHandler<SendVerificationEmailCommand, Result<SendVerificationEmailData>>
{
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IEmailService _emailService;

    public SendVerificationEmailCommandHandler(
        IEmailVerificationRepository emailVerificationRepository,
        IEmailService emailService)
    {
        _emailVerificationRepository = emailVerificationRepository;
        _emailService = emailService;
    }

    public async Task<Result<SendVerificationEmailData>> HandleAsync(SendVerificationEmailCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingVerification = await _emailVerificationRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingVerification != null && existingVerification.IsVerified)
            {
                return Result<SendVerificationEmailData>.Error((int)EmailErrorCode.EmailAlreadyVerified);
            }

            var verificationCode = VerificationCodeGenerator.GenerateSecureCode(6);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            var emailVerification = new EmailVerification
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                Email = command.Email,
                VerificationCode = verificationCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                IsVerified = false,
                Attempts = 0
            };

            await _emailVerificationRepository.CreateAsync(emailVerification, cancellationToken);

            var emailSent = await _emailService.SendVerificationEmailAsync(
                command.Email,
                command.Username,
                verificationCode,
                cancellationToken);

            if (!emailSent)
            {
                return Result<SendVerificationEmailData>.Error((int)EmailErrorCode.EmailSendFailed);
            }

            var resultData = new SendVerificationEmailData
            {
                VerificationId = emailVerification.Id,
                ExpiresAt = expiresAt
            };

            return Result<SendVerificationEmailData>.Success(resultData, (int)EmailSuccessCode.VerificationEmailSent);
        }
        catch (Exception)
        {
            return Result<SendVerificationEmailData>.Error((int)EmailErrorCode.DatabaseError);
        }
    }
}
