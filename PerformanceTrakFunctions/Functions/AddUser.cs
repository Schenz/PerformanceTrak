using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PerformanceTrakFunctions.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceTrakFunctions.Functions
{
    public static class AddUser
    {
        [FunctionName("AddUser")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUser")] HttpRequest req, ILogger log)
        {
            try
            {
                var user = Util.Security.ValidateUser(req);
                if (user == null)
                {
                    return new ForbidResult("Bearer");
                }

                var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult(new { error = true, message = "User not passed" });
                }

                var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT"));
                var environmentString = $"{environment.ToString()}";
                var tableName = $"Users{(environment != Environments.PROD ? environmentString : string.Empty)}";
                var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("TABLESTORECONNECTIONSTRING"))
                    .CreateCloudTableClient()
                    .GetTableReference(tableName);

                await table.CreateIfNotExistsAsync();

                entity.PartitionKey = entity.FamilyName.Substring(0, 1);
                entity.RowKey = entity.Id.ToString();

                var result = await table.ExecuteAsync(TableOperation.Insert(entity));

                return new CreatedResult("", entity);
            }
            catch (StorageException ex)
            {
                var message = ex.Message;
                if (message.StartsWith("Conflict"))
                {
                    log.LogCritical(ex, "Record Already Exists");
                    return new ConflictObjectResult(new { error = true, message = "User Already Exists" });
                }
                else
                {
                    log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                    return new BadRequestObjectResult("Error While Adding User");
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (message.StartsWith("IDX10223: Lifetime validation failed"))
                {
                    log.LogCritical(ex, "Access Token Expired");
                    return new ForbidResult("Bearer");
                }
                else
                {
                    log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                    return new BadRequestObjectResult("Error While Adding User");
                }
            }
        }
    }
}
