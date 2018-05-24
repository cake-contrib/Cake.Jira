using Newtonsoft.Json;

namespace Cake.Jira.Dtos
{
    public abstract class JiraSettings
    {
        [JsonIgnore]
        public string Host { get; set; }
        [JsonIgnore]
        public string User { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}