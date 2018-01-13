using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Jira.Dtos;

namespace Cake.Jira
{
    [CakeAliasCategory("Jira")]
    public static class MethodAliases
    {
        [CakeMethodAlias]
        public static async Task CreateJiraVersion(this ICakeContext context, CreateJiraVersionSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Host))
            {
                throw new ArgumentException("Host must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.Project))
            {
                throw new ArgumentException("Project must be specified.");
            }

            if (string.IsNullOrWhiteSpace(settings.Number))
            {
                throw new ArgumentException("Version Number must be specified.");
            }
            await JiraClient.CreateJiraVersion(settings, context.Log);
        }
    }
}
