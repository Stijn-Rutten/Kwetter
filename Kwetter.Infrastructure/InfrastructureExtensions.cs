using Kwetter.Core;
using Kwetter.Domain.Aggregates;
using Kwetter.Domain.ValueObjects;
using Kwetter.Infrastructure.Data;
using Kwetter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kwetter.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KwetterDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Database"))
        );


        services.AddTransient<IEventSourceRepository<UserId, User>, UserEventSourceRepository>();
        return services;
    }
}
