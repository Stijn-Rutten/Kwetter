using Kwetter.Core;
using Kwetter.Domain.Aggregates;
using Kwetter.Domain.ValueObjects;
using Kwetter.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Kwetter.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IEventSourceRepository<UserId, User>, StubUserEventSourceRepository>();

        return services;
    }
}
