using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<TableResult> Add(UserEntity entity);

        Task<UserEntity> Get(string partitionKey, string rowKey);
    }
}