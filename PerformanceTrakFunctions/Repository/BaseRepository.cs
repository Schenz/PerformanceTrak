using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class BaseRepository
    {
        public static async Task<CloudTable> GetTable(string tName)
        {
            var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT"));
            var environmentString = $"{environment.ToString()}";
            var tableName = $"{tName}{(environment != Environments.PROD ? environmentString : string.Empty)}";
            var table = CloudStorageAccount
                .Parse(Environment.GetEnvironmentVariable("TABLESTORECONNECTIONSTRING"))
                .CreateCloudTableClient()
                .GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}