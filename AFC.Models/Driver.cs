using Newtonsoft.Json;

namespace AFC.Models
{
    public class Driver
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}