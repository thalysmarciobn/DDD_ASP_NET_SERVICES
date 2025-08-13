using Common.CQRS;
using Email.Application.Commands;
using Email.Application.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        
        services.AddScoped<ICommandHandler<SendVerificationEmailCommand, Result<Email.Application.Common.SendVerificationEmailData>>, SendVerificationEmailCommandHandler>();
        services.AddScoped<ICommandHandler<ResendVerificationEmailCommand, Result<Email.Application.Common.ResendVerificationEmailData>>, ResendVerificationEmailCommandHandler>();

        return services;
    }
}
