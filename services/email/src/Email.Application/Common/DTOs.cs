namespace Email.Application.Common;

public record SendVerificationEmailData
{
    public Guid VerificationId { get; init; }
    public DateTime ExpiresAt { get; init; }
}

public record ResendVerificationEmailData
{
    public Guid VerificationId { get; init; }
    public DateTime ExpiresAt { get; init; }
    public int Attempts { get; init; }
}

public record EmailVerificationData
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public bool IsVerified { get; init; }
    public DateTime ExpiresAt { get; init; }
    public int Attempts { get; init; }
}
