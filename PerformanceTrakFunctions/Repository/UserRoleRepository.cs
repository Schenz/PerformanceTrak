using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public async Task<TableResult> Add(UserRoleEntity entity) => await (await GetTable("UserRoles")).ExecuteAsync(TableOperation.Insert(entity));

        public async Task<TableQuerySegment<UserRoleEntity>> Get(string userId)
        {
            CloudTable cloudTable = await GetTable("UserRoles");

            TableQuery<UserRoleEntity> userIdQuery = new TableQuery<UserRoleEntity>().Where(TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId));

            return await cloudTable.ExecuteQuerySegmentedAsync(userIdQuery, new TableContinuationToken());
        }
        
        public async Task<TableResult> Update(UserRoleEntity entity)
        {
            var table = await GetTable("UserRoles");

            entity.ETag = "*";
            var updatedEntity = await table.ExecuteAsync(TableOperation.Merge(entity));

            return updatedEntity;
        }
    }
}