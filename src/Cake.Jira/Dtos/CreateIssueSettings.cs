using System;
using System.Collections.Generic;

namespace Cake.Jira.Dtos
{
    public class CreateIssueSettings : JiraSettings
    {
        public string Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public string Environment { get; set; }
        public string Assignee { get; set; }
        public int Priority { get; set; }
        public int Type { get; set; }
        public List<string> Labels { get; set; }
        public DateTime? DueDate { get; set; }
    }
}