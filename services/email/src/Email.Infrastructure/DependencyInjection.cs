using Email.Domain.Repositories;
using Email.Domain.Services;
using Email.Infrastructure.Data;
using Email.Infrastructure.Repositories;
using Email.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Email.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EmailDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(EmailDbContext).Assembly.FullName)));

        services.AddSingleton<IConnectionFactory>(provider =>
        {
            var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "192.168.10.180";
            var rabbitMqPort = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
            var rabbitMqUsername = configuration["RabbitMQ:Username"] ?? "guest";
            var rabbitMqPassword = configuration["RabbitMQ:Password"] ?? "guest";

            return new ConnectionFactory
            {
                HostName = rabbitMqHost,
                Port = rabbitMqPort,
                UserName = rabbitMqUsername,
                Password = rabbitMqPassword,
                VirtualHost = "/"
            };
        });

        services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddHostedService<MessageQueueService>();

        return services;
    }
}
