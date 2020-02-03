using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public async Task<TableResult> Add(UserRoleEntity entity) => await (await GetTable()).ExecuteAsync(TableOperation.Insert(entity));

        public async Task<UserRoleEntity> Get(string partitionKey, string rowKey) => (UserRoleEntity)(await (await GetTable()).ExecuteAsync(TableOperation.Retrieve<UserRoleEntity>(partitionKey, rowKey))).Result;

        public async Task<TableResult> Update (UserRoleEntity entity) {
            var table = await GetTable();
            
            entity.ETag = "*";
            var updatedEntity = await table.ExecuteAsync(TableOperation.Merge(entity));

            return updatedEntity;
        }
    }
}