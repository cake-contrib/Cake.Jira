namespace Cake.Jira.Dtos
{
    public class MigrateIssuesVersionSettings : JiraSettings
    {
        public string Project { get; set; }
        public string FromVersion { get; set; }
        public string ToVersion { get; set; }
    }
}
