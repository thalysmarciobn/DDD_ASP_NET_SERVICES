using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Common.Messages.Events;
using Common.Messages.Constants;

namespace Auth.Infrastructure.Services;

public interface IMessageQueueService
{
    void PublishUserCreated(Guid userId, string email, string username);
}

public class MessageQueueService : IMessageQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<MessageQueueService> _logger;

    public MessageQueueService(
        IConnectionFactory connectionFactory,
        ILogger<MessageQueueService> logger)
    {
        _logger = logger;
        
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(MessageConstants.Exchanges.UserEvents, ExchangeType.Topic, durable: true);
    }

    public void PublishUserCreated(Guid userId, string email, string username)
    {
        try
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                UserId = userId,
                Email = email,
                Username = username
            };

            var message = JsonSerializer.Serialize(userCreatedEvent);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: MessageConstants.Exchanges.UserEvents,
                routingKey: MessageConstants.RoutingKeys.UserCreated,
                basicProperties: null,
                body: body);

            _logger.LogInformation("User created event published for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing user created event for user {UserId}", userId);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
