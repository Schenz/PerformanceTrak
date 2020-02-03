using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PerformanceTrakFunctions.Models;
using Microsoft.WindowsAzure.Storage;
using PerformanceTrakFunctions.Security;
using PerformanceTrakFunctions.Repository;

namespace PerformanceTrakFunctions.Functions
{
    public class AddUser
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRepository _userRepository;

        public AddUser(IAccessTokenProvider tokenProvider, IUserRepository userRepository)
        {
            _tokenProvider = tokenProvider;
            _userRepository = userRepository;
        }

        [FunctionName("AddUser")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUser")] HttpRequest req, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }
                
                var entity = JsonConvert.DeserializeObject<UserEntity>(new StreamReader(req.Body).ReadToEnd());

                if (entity == null)
                {
                    return new BadRequestObjectResult(new { error = true, message = "User not passed" });
                }

                entity.PartitionKey = entity.FamilyName.Substring(0, 1);
                entity.RowKey = entity.Id.ToString();

                return new CreatedResult("", _userRepository.Add(entity).Result);
            }
            catch (StorageException ex)
            {
                var message = ex.Message;
                if (message.StartsWith("Conflict"))
                {
                    log.LogCritical(ex, "Record Already Exists");
                    return new ConflictObjectResult(new { error = true, message = "User Already Exists" });
                }
                
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
