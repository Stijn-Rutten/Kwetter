using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Kwetter.Api.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddKwetterAuthentication(this IServiceCollection services)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

        return services;
    }
}
