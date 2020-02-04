using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<TableResult> Add(UserEntity entity, string tableName);

        Task<UserEntity> Get(string partitionKey, string rowKey);

        Task<TableResult> Update(UserEntity entity);
    }
}