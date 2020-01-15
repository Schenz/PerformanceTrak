using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<TableResult> Add(UserEntity entity) => await (await GetTable()).ExecuteAsync(TableOperation.Insert(entity));

        public async Task<UserEntity> Get(string partitionKey, string rowKey) => (UserEntity)(await (await GetTable()).ExecuteAsync(TableOperation.Retrieve<UserEntity>(partitionKey, rowKey))).Result;

        private static async Task<CloudTable> GetTable()
        {
            var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT"));
            var environmentString = $"{environment.ToString()}";
            var tableName = $"Users{(environment != Environments.PROD ? environmentString : string.Empty)}";
            var table = CloudStorageAccount
                .Parse(Environment.GetEnvironmentVariable("TABLESTORECONNECTIONSTRING"))
                .CreateCloudTableClient()
                .GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();
            
            return table;
        }
    }
}