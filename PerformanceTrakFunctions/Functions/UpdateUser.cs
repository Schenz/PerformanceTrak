using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using PerformanceTrakFunctions.Models;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Security;

namespace PerformanceTrakFunctions.Functions
{
    public class UpdateUser
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRepository _userRepository;

        public UpdateUser(IAccessTokenProvider tokenProvider, IUserRepository userRepository)
        {
            _tokenProvider = tokenProvider;
            _userRepository = userRepository;
        }

        [FunctionName("UpdateUser")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "UpdateUser")] HttpRequest req, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }
                
                var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult(new { error = true, message = "User not passed" });
                }

                entity.PartitionKey = entity.FamilyName.Substring(0, 1);
                entity.RowKey = entity.Id.ToString();

                var value = await _userRepository.Update(entity);
                return new OkObjectResult(value);
            }
            catch (StorageException ex)
            {
                log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding User");
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}