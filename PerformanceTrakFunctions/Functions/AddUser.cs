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
using PerformanceTrakFunctions.Util;

namespace PerformanceTrakFunctions.Functions
{
    public class AddUser
    {
        private readonly IAccessTokenProvider _tokenProvider;

        public AddUser(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        [FunctionName("AddUser")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUser")] HttpRequest req, ILogger log)
        {
            try
            {
                var result = _tokenProvider.ValidateToken(req);

                if (result.Status == AccessTokenStatus.Valid)
                {
                    var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                    if (entity == null)
                    {
                        return new BadRequestObjectResult(new { error = true, message = "User not passed" });
                    }

                    // TODO: Implement a repository pattern so that I can inject different mocks or error conditions for testing
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

                    await table.ExecuteAsync(TableOperation.Insert(entity));

                    return new CreatedResult("", entity);
                }
                else
                {
                    return new UnauthorizedResult();
                }
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
                log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}
