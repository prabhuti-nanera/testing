using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CRC.WebPortal.Application.Interfaces;
using CRC.WebPortal.Domain.Interfaces;
using CRC.WebPortal.Infrastructure.Data;
using CRC.WebPortal.Infrastructure.Repositories;
using CRC.WebPortal.Infrastructure.Services;

namespace CRC.WebPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
