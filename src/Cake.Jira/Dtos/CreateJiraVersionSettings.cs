using System;

namespace Cake.Jira.Dtos
{
    public class CreateJiraVersionSettings : JiraSettings
    {
        public string Number { get; set; }
        public string Prefix { get; set; }
        public bool IsReleased { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}