using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace PerformanceTrakFunctions.Models
{
    public class UserEntity : TableEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "family_name")]
        public string FamilyName { get; set; }

        [JsonProperty(PropertyName = "given_name")]
        public string GivenName { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
