using Kwetter.Core;
using Kwetter.Domain.Aggregates;
using Kwetter.Domain.ValueObjects;
using Kwetter.Infrastructure.Data;
using Kwetter.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kwetter.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplication AddInfrastructure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContext<KwetterDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Database"))
        );

        builder.Services.AddTransient<IEventSourceRepository<UserId, User>, UserEventSourceRepository>();

        builder.Host.UseSerilog((context, logContext) =>
        {
            logContext
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithMachineName();
        });
        
        return builder;
    }
}
