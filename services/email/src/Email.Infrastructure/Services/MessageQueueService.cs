using Common.CQRS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Email.Application.Commands;
using Email.Application.Common;
using Common.Messages.Events;
using Common.Messages.Constants;

namespace Email.Infrastructure.Services;

public class MessageQueueService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MessageQueueService> _logger;

    public MessageQueueService(
        IConnectionFactory connectionFactory,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MessageQueueService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(MessageConstants.Exchanges.UserEvents, ExchangeType.Topic, durable: true);
        _channel.QueueDeclare(MessageConstants.QueueNames.EmailVerificationQueue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(MessageConstants.QueueNames.EmailVerificationQueue, MessageConstants.Exchanges.UserEvents, MessageConstants.RoutingKeys.UserCreated);
        
        _channel.BasicQos(0, 1, false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                _logger.LogInformation("Received message: {Message}", message);
                
                var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);
                if (userCreatedEvent != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
                    
                    var command = new SendVerificationEmailCommand
                    {
                        UserId = userCreatedEvent.UserId,
                        Email = userCreatedEvent.Email,
                        Username = userCreatedEvent.Username
                    };

                    var result = await dispatcher.SendAsync<SendVerificationEmailCommand, Result<SendVerificationEmailData>>(command);
                    
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("Email verification sent successfully for user {UserId}", userCreatedEvent.UserId);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _logger.LogError("Failed to send email verification for user {UserId}: {ErrorCode}", 
                            userCreatedEvent.UserId, result.ErrorCode);
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: MessageConstants.QueueNames.EmailVerificationQueue, autoAck: false, consumer: consumer);
        
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
