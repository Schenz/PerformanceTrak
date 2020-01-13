using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Models;

namespace PerformanceTrakFunctions.Repository
{
    public interface IUserRepository : IRepository
    {
        TableResult Add(UserEntity entity);
    }
}