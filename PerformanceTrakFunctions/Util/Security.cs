using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PerformanceTrakFunctions.Util
{
    public static class Security
    {
        public static ClaimsPrincipal ValidateUser(HttpRequest req)
        {
            if (!req.Headers.Any(t => t.Key == "Authorization"))
            {
                return null;
            }

            var headerValue = req.Headers.First(t => t.Key == "Authorization").Value;
            var bearerValue = headerValue.FirstOrDefault(v => v.StartsWith("Bearer ")) ?? String.Empty;
            if (String.IsNullOrWhiteSpace(bearerValue))
            {
                return null;

            }
            var bearerToken = bearerValue.Split(' ')[1];

            var principal = PerformanceTrakFunctions.Util.Security.ValidateToken(bearerToken, "https://scdperformancetrak.b2clogin.com/e97d1dfb-9447-4fbd-9f09-eb4c98ef0a16/v2.0/", "pt");
            if (principal == null)
            {
                return null;
            }

            return principal;
        }

        private static ClaimsPrincipal ValidateToken(string jwtToken, string issuer, string requiredScope)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(jwtToken))
            {
                return null;
            }

            handler.InboundClaimTypeMap.Clear();

            Microsoft.IdentityModel.Tokens.SecurityToken token;
            var principal = handler.ValidateToken(jwtToken, new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidIssuer = issuer,
                ValidateIssuerSigningKey = false,
                SignatureValidator = (t, param) => new JwtSecurityToken(t),
                NameClaimType = "sub"

            }, out token);

            if (!principal.Claims.Any(c => c.Type == "scp" && c.Value == requiredScope))
            {
                return null;
            }

            return principal;
        }

    }
}