using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMessageQueueService, MessageQueueService>();

        return services;
    }
}
