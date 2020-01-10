using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace PerformanceTrakFunctions.Util
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private readonly string _audience;
        private readonly string _issuer;

        public AccessTokenProvider(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;
        }

        public AccessTokenResult ValidateToken(HttpRequest req)
        {
            try
            {
                // Get the token from the header
                if (req != null &&
                    req.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                    req.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                {
                    var token = req.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);
                    
                    // Create the parameters
                    var tokenParams = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = _audience,
                        ValidIssuer = _issuer,
                        ValidateIssuerSigningKey = false,
                        SignatureValidator = (t, param) => new JwtSecurityToken(t),
                        ValidateLifetime = true,
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    };

                    // Validate the token
                    var handler = new JwtSecurityTokenHandler();
                    var result = handler.ValidateToken(token, tokenParams, out var securityToken);
                    return AccessTokenResult.Success(result);
                }
                else
                {
                    return AccessTokenResult.NoToken();
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                return AccessTokenResult.Error(ex);
            }
        }
    }
}