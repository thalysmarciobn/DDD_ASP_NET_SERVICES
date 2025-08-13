using Email.Domain.Services;

namespace Email.Infrastructure.Services;

public class EmailService : IEmailService
{
    public async Task<bool> SendVerificationEmailAsync(string to, string username, string verificationCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var body = $@"
                <html>
                <body>
                    <h2>Olá {username}!</h2>
                    <p>Seu código de verificação é: <strong>{verificationCode}</strong></p>
                    <p>Este código expira em 24 horas.</p>
                    <p>Se você não solicitou esta verificação, ignore este email.</p>
                </body>
                </html>";

            await Task.Delay(100, cancellationToken);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendResendVerificationEmailAsync(string to, string username, string verificationCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var body = $@"
                <html>
                <body>
                    <h2>Olá {username}!</h2>
                    <p>Seu novo código de verificação é: <strong>{verificationCode}</strong></p>
                    <p>Este código expira em 24 horas.</p>
                    <p>Se você não solicitou este reenvio, ignore este email.</p>
                </body>
                </html>";

            await Task.Delay(100, cancellationToken);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
}
