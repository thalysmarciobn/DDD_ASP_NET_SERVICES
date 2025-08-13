using Common.CQRS;
using Auth.Application.Commands;
using Auth.Application.Handlers;
using Auth.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        
        services.AddScoped<ICommandHandler<RegisterUserCommand, Result<Auth.Application.Common.RegisterUserData>>, RegisterUserCommandHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, Result<Auth.Application.Common.LoginUserData>>, LoginCommandHandler>();
        services.AddScoped<IQueryHandler<GetUserQuery, Result<Auth.Application.Common.UserData>>, GetUserQueryHandler>();

        return services;
    }
}
