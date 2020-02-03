using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace PerformanceTrakFunctions.Models
{
    public class UserRoleEntity : TableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }
    }
}