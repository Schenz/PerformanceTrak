using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace PerformanceTrakFunctions.Models
{
    public class UserRoleEntity : TableEntity
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }
    }
}