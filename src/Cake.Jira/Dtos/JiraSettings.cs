namespace Cake.Jira.Dtos
{
    public abstract class JiraSettings
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}