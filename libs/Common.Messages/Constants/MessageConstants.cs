namespace Common.Messages.Constants;

public static class MessageConstants
{
    public static class Exchanges
    {
        public const string UserEvents = "user_events";
        public const string EmailEvents = "email_events";
    }

    public static class RoutingKeys
    {
        public const string UserCreated = "user.created";
        public const string UserUpdated = "user.updated";
        public const string UserDeleted = "user.deleted";
        public const string EmailSent = "email.sent";
        public const string EmailVerificationCompleted = "email.verification.completed";
    }

    public static class QueueNames
    {
        public const string EmailVerificationQueue = "email_verification_queue";
        public const string UserEventsQueue = "user_events_queue";
    }
}
