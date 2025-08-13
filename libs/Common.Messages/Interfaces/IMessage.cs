namespace Common.Messages.Interfaces;

public interface IMessage
{
    Guid Id { get; set; }
    DateTime Timestamp { get; set; }
    string MessageType { get; set; }
}

public interface IEvent : IMessage
{
    string EventName { get; set; }
    string EventVersion { get; set; }
}
