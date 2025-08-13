using Common.Messages.Interfaces;

namespace Common.Messages.Events;

public class UserCreatedEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = "Event";
    public string EventName { get; set; } = "UserCreated";
    public string EventVersion { get; set; } = "1.0";
    
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class UserUpdatedEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = "Event";
    public string EventName { get; set; } = "UserUpdated";
    public string EventVersion { get; set; } = "1.0";
    
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class UserDeletedEvent : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = "Event";
    public string EventName { get; set; } = "UserDeleted";
    public string EventVersion { get; set; } = "1.0";
    
    public Guid UserId { get; set; }
    public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}
