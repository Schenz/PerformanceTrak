using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public interface IUserRoleRepository : IRepository
    {
        Task<TableResult> Add(UserRoleEntity entity);

        Task<TableQuerySegment<UserRoleEntity>> Get(string userId);

        Task<TableResult> Update(UserRoleEntity entity);
    }
}