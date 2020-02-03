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
    public class AddUserRole
    {
        private readonly IAccessTokenProvider _tokenProvider;

        private readonly IUserRoleRepository _userRoleRepository;

        public AddUserRole(IAccessTokenProvider tokenProvider, IUserRoleRepository userRoleRepository)
        {
            _tokenProvider = tokenProvider;
            _userRoleRepository = userRoleRepository;
        }

        [FunctionName("AddUserRole")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUserRole")] HttpRequest req, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }
                
                var entity = JsonConvert.DeserializeObject<UserRoleEntity>(new StreamReader(req.Body).ReadToEnd());

                if (entity == null)
                {
                    return new BadRequestObjectResult(new { error = true, message = "User not passed" });
                }

                entity.PartitionKey = entity.RoleName;
                entity.RowKey = entity.UserId.ToString();

                return new CreatedResult("", _userRoleRepository.Add(entity).Result);
            }
            catch (StorageException ex)
            {
                var message = ex.Message;
                if (message.StartsWith("Conflict"))
                {
                    log.LogCritical(ex, "Record Already Exists");
                    return new ConflictObjectResult(new { error = true, message = "UserRole Already Exists" });
                }
                
                log.LogCritical(ex, $"Error Adding UserRole: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding UserRole");
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Error Adding UserRole: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding UserRole");
            }
        }
    }
}
