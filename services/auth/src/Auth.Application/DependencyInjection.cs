using Auth.Application.CQRS;
using Auth.Application.Commands;
using Auth.Application.Queries;
using Auth.Application.Handlers;
using Auth.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        
        services.AddScoped<ICommandHandler<RegisterUserCommand, Result<RegisterUserData>>, RegisterUserCommandHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, Result<LoginUserData>>, LoginCommandHandler>();
        
        services.AddScoped<IQueryHandler<GetUserQuery, Result<UserData>>, GetUserQueryHandler>();
        
        return services;
    }
}
