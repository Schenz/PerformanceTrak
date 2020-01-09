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
                // get headers
                var headers = req.Headers;
                log.LogInformation("HEADERS:");
                foreach (var header in headers)
                {
                    log.LogInformation($"{header.Key}: {header.Value}");
                }
                log.LogInformation("");

                // get cookies
                var cookies = req.Cookies;
                log.LogInformation("COOKIES:");
                foreach (var cookie in cookies)
                {
                    log.LogInformation($"{cookie.Key}: {cookie.Value}");
                }
                log.LogInformation("");
                
                var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult("User not passed");
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
            catch (Exception ex)
            {
                log.LogCritical(ex, $"Error Adding User: {ex.Message}");
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}
