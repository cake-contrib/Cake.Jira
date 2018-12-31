using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Jira.Dtos;

namespace Cake.Jira
{
    [CakeAliasCategory("Jira")]
    [CakeNamespaceImport("Cake.Jira.Dtos")]
    public static class MethodAliases
    {
        [CakeMethodAlias]
        public static async Task CreateOrUpdateJiraVersion(this ICakeContext context, CreateOrUpdateJiraVersionSettings settings)
        {
            ValidateJiraSettings(settings);

            if (string.IsNullOrWhiteSpace(settings.Project))
            {
                throw new ArgumentException("Project must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.VersionName))
            {
                throw new ArgumentException("Version Number must be specified.");
            }
            await JiraClient.CreateOrUpdateJiraVersion(settings, context.Log);
        }

        [CakeMethodAlias]
        public static async Task MigrateIssuesVersion(this ICakeContext context, MigrateIssuesVersionSettings settings)
        {
            ValidateJiraSettings(settings);

            if (string.IsNullOrWhiteSpace(settings.Project))
            {
                throw new ArgumentException("Project must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.FromVersion))
            {
                throw new ArgumentException("The source version must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.ToVersion))
            {
                throw new ArgumentException("The target version must be specified.");
            }

            await JiraClient.MigrateIssuesVersion(settings, context.Log);
        }

        [CakeMethodAlias]
        public static async Task CreateJiraIssue(this ICakeContext context, CreateIssueSettings settings)
        {
            ValidateJiraSettings(settings);

            if (string.IsNullOrWhiteSpace(settings.Project))
            {
                throw new ArgumentException("Project must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.Summary))
            {
                throw new ArgumentException("Summary must be specified.");
            }

            await JiraClient.CreateJiraIssue(settings, context.Log);
        }

        private static void ValidateJiraSettings(JiraSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Host))
            {
                throw new ArgumentException("Host must be specified.");
            }
        }
    }
}
