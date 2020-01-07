using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using Newtonsoft.Json;

namespace AddUser
{
    public static class AddUser
    {
        [FunctionName("AddUser")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddUser")] HttpRequest req, ILogger log)
        {
            try
            {
                var entity = JsonConvert.DeserializeObject<UserEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult("User not passed");
                }

                var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("Environment"));
                var environmentString = $"{environment.ToString()}";
                var tableName = $"user{(environment != Environments.PROD ? environmentString : string.Empty)}";

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
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}
