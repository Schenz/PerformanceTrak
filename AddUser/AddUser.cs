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
                var entity = JsonConvert.DeserializeObject<EmployeeSessionEntity>(await new StreamReader(req.Body).ReadToEndAsync());

                if (entity == null)
                {
                    return new BadRequestObjectResult("User not passed");
                }

                var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"))
                    .CreateCloudTableClient()
                    .GetTableReference("employee");

                await table.CreateIfNotExistsAsync();

                entity.PartitionKey = entity.Id.ToString();
                entity.RowKey = entity.Name;

                var result = await table.ExecuteAsync(TableOperation.Insert(entity));

                return (ActionResult)new CreatedResult("", entity);
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, "Error While Adding User");
                return new BadRequestObjectResult("Error While Adding User");
            }
        }
    }
}
