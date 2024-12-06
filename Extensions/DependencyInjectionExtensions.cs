using DatingApp.Interfaces;
using DatingApp.Services;

namespace DatingApp.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        return services;   
    }
}
