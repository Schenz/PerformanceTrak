using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using PerformanceTrakFunctions.Models;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Security;

namespace PerformanceTrakFunctions.Util
{
    internal static class NewUser {
        internal static IActionResult Add(IAccessTokenProvider tokenProvider, IUserRepository userRepository, HttpRequest req, ILogger log, string table)
        {
            try
            {
                var result = tokenProvider.ValidateToken(req);

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

                return new CreatedResult("", userRepository.Add(entity, table).Result);
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