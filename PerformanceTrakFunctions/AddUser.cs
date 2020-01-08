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

namespace PerformanceTrakFunctions
{
    public static class AddUser
    {
        [FunctionName("AddUser")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUser")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("Deserialize User");
                Console.WriteLine("Deserialize User");
                var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult("User not passed");
                }

                log.LogInformation("Get Table Name based on Environment");
                Console.WriteLine("Get Table Name based on Environment");
                var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("Environment"));
                var environmentString = $"{environment.ToString()}";
                var tableName = $"user{(environment != Environments.PROD ? environmentString : string.Empty)}";
                log.LogInformation($"tableName: {tableName}");
                Console.WriteLine($"tableName: {tableName}");

                var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("TableStoreConnectionString"))
                    .CreateCloudTableClient()
                    .GetTableReference(tableName);

                await table.CreateIfNotExistsAsync();

                entity.PartitionKey = entity.FamilyName.Substring(0, 1);
                entity.RowKey = entity.Id.ToString();

                var result = await table.ExecuteAsync(TableOperation.Insert(entity));

                return (ActionResult)new CreatedResult("", entity);
            }
            catch (System.Exception ex)
            {
                log.LogCritical(ex, "Error Adding User");
                Console.WriteLine(ex);
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}
