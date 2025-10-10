using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CRC.WebPortal.Application;

/// <summary>
/// Application layer dependency injection - ONLY business logic services
/// No UI concerns, no validation - pure business logic only
/// </summary>
public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        RegisterAutoMapper(services);
        RegisterMediatR(services);
        
        return services;
    }

    #endregion

    #region Private Registration Methods

    /// <summary>
    /// Register AutoMapper for object mapping
    /// </summary>
    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Register MediatR for CQRS pattern - pure business logic only
    /// No validation behaviors - validation is UI concern
    /// </summary>
    private static void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // No validation behavior - validation is UI layer responsibility
        });
    }

    #endregion
}
