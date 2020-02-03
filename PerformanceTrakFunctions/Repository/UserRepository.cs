using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public async Task<TableResult> Add(UserEntity entity) => await (await GetTable()).ExecuteAsync(TableOperation.Insert(entity));

        public async Task<UserEntity> Get(string partitionKey, string rowKey) => (UserEntity)(await (await GetTable()).ExecuteAsync(TableOperation.Retrieve<UserEntity>(partitionKey, rowKey))).Result;

        public async Task<TableResult> Update (UserEntity entity) {
            var table = await GetTable();
            
            entity.ETag = "*";
            var updatedEntity = await table.ExecuteAsync(TableOperation.Merge(entity));

            return updatedEntity;
        }
    }
}