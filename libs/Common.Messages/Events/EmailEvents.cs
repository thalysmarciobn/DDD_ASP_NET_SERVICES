using Common.Messages.Interfaces;

namespace Common.Messages.Events;

public class EmailSentEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = "Event";
    public string EventName { get; set; } = "EmailSent";
    public string EventVersion { get; set; } = "1.0";
    
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string EmailType { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class EmailVerificationCompletedEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = "Event";
    public string EventName { get; set; } = "EmailVerificationCompleted";
    public string EventVersion { get; set; } = "1.0";
    
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime VerifiedAt { get; set; } = DateTime.UtcNow;
}
