using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Security;

[assembly: FunctionsStartup(typeof(PerformanceTrakFunctions.Startup))]

namespace PerformanceTrakFunctions
{
    /// <summary>
    /// Runs when the Azure Functions host starts.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Get the configuration files for the OAuth token issuer
            var audience = Environment.GetEnvironmentVariable("Audience");
            var issuer = Environment.GetEnvironmentVariable("Issuer");

            // Register the access token provider as a singleton
            builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>(s => new AccessTokenProvider(audience, issuer));
            builder.Services.AddSingleton<IUserRepository, UserRepository>(s => new UserRepository());
            builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>(s => new UserRoleRepository());
            builder.Services.AddSingleton<ISendGridClient, MySendGridClient>(s => new MySendGridClient());
        }
    }
}
