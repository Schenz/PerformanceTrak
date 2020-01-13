using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
