using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public async Task<TableResult> Add(UserEntity entity) => await (await GetTable("Users")).ExecuteAsync(TableOperation.Insert(entity));

        public async Task<UserEntity> Get(string partitionKey, string rowKey) => (UserEntity)(await (await GetTable("Users")).ExecuteAsync(TableOperation.Retrieve<UserEntity>(partitionKey, rowKey))).Result;

        public async Task<TableResult> Update (UserEntity entity) {
            var table = await GetTable("Users");
            
            entity.ETag = "*";
            var updatedEntity = await table.ExecuteAsync(TableOperation.Merge(entity));

            return updatedEntity;
        }
    }
}