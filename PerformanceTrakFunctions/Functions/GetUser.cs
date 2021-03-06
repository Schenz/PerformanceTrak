using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PerformanceTrakFunctions.Security;
using PerformanceTrakFunctions.Repository;
using Newtonsoft.Json;

namespace PerformanceTrakFunctions.Functions
{
    public class GetUser
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRepository _userRepository;

        public GetUser(IAccessTokenProvider tokenProvider, IUserRepository userRepository)
        {
            _tokenProvider = tokenProvider;
            _userRepository = userRepository;
        }

        [FunctionName("GetUser")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetUser/{partitionKey?}/{rowKey?}")] HttpRequest req, string partitionKey, string rowKey, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                if (string.IsNullOrWhiteSpace(partitionKey) || string.IsNullOrWhiteSpace(rowKey))
                {
                    return new BadRequestObjectResult(new { error = true, message = "Please Pass both PartitionKey and RowKey" });
                }

                return new OkObjectResult(_userRepository.Get(partitionKey, rowKey).Result);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Error Getting User: {ex.Message}");
                return new BadRequestObjectResult("Error While Getting User");
            }
        }
    }
}
