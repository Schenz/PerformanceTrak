using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Security;

namespace PerformanceTrakFunctions.Functions
{
    public class GetUserRole
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRoleRepository _userRoleRepository;

        public GetUserRole(IAccessTokenProvider tokenProvider, IUserRoleRepository userRoleRepository)
        {
            _tokenProvider = tokenProvider;
            _userRoleRepository = userRoleRepository;
        }

        [FunctionName("GetUserRole")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetUserRole/{userId?}")] HttpRequest req, string userId, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new BadRequestObjectResult(new { error = true, message = "Please Pass both UserId" });
                }

                return new OkObjectResult(_userRoleRepository.Get(userId).Result);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Error Getting UserRole: {ex.Message}");
                return new BadRequestObjectResult("Error While Getting UserRole");
            }
        }
    }
}