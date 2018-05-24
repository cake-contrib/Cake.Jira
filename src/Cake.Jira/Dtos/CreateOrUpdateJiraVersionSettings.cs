using System;

namespace Cake.Jira.Dtos
{
    public class CreateOrUpdateJiraVersionSettings : JiraSettings
    {
        public string VersionName { get; set; }
        public bool IsReleased { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}