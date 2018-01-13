using System;
using System.Threading.Tasks;
using Atlassian.Jira;
using Cake.Core.Diagnostics;
using Cake.Jira.Dtos;
using Cake.Jira.Helpers;
using Newtonsoft.Json;

namespace Cake.Jira
{
    public class JiraClient
    {
        public static async Task CreateJiraVersion(CreateJiraVersionSettings settings, ICakeLog logger)
        {
            var versionName = $"{settings.Prefix}{settings.Number}";
            logger.Verbose($"Creating version {versionName} on {settings.Project}");
            var jira = Atlassian.Jira.Jira.CreateRestClient(settings.Host, settings.User, settings.Password);
            var version = await jira.Versions.GetVersionAsync(versionName);
            
            if(version == null)
            {
                logger.Verbose($"{versionName} not found. Creating it...");
                logger.Debug($"Creating version with paramenters {Environment.NewLine}{JsonConvert.SerializeObject(settings)}");
                await jira.Versions.CreateVersionAsync(
                    new ProjectVersionCreationInfo($"{settings.Prefix}{settings.Number}")
                    {
                        Description = settings.Description,
                        IsReleased = settings.IsReleased,
                        StartDate = DateTime.UtcNow,
                        ProjectKey = settings.Project
                    }
                );
            }
            else
            {
                logger.Verbose($"{versionName} already exists. Updating it...");
                logger.Debug($"Updating version with paramenters {Environment.NewLine}{JsonConvert.SerializeObject(settings)}");
                await version.Update(settings);
            }
            logger.Information($"Created version {versionName} on {settings.Project}");
        }
    }
}
