using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRepository : IUserRepository
    {
        public TableResult Add(UserEntity entity)
        {
            var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT"));
            var environmentString = $"{environment.ToString()}";
            var tableName = $"Users{(environment != Environments.PROD ? environmentString : string.Empty)}";
            var table = CloudStorageAccount
                .Parse(Environment.GetEnvironmentVariable("TABLESTORECONNECTIONSTRING"))
                .CreateCloudTableClient()
                .GetTableReference(tableName);

            table.CreateIfNotExistsAsync().Wait();

            return table.ExecuteAsync(TableOperation.Insert(entity)).Result;
        }
    }
}