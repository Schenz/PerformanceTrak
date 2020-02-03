using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public interface IUserRoleRepository : IRepository
    {
        Task<TableResult> Add(UserRoleEntity entity);

        Task<UserRoleEntity> Get(string partitionKey, string rowKey);

        Task<TableResult> Update(UserRoleEntity entity);
    }
}