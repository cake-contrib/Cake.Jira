using System;
using System.Linq;
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
        public static async Task CreateOrUpdateJiraVersion(CreateOrUpdateJiraVersionSettings settings, ICakeLog logger)
        {
            try
            {
                logger.Information($"Creating version {settings.VersionName} on {settings.Project}");
                var jira = Atlassian.Jira.Jira.CreateRestClient(settings.Host, settings.User, settings.Password);
                var versions = await jira.Versions.GetVersionsAsync(settings.Project);
                var version = versions.SingleOrDefault(v => v.Name == settings.VersionName);

                if (version == null)
                {
                    logger.Information($"{settings.VersionName} not found. Creating it...");
                    logger.Information(
                        $"Creating version with paramenters: {Environment.NewLine}{JsonConvert.SerializeObject(settings)}");
                    await jira.Versions.CreateVersionAsync(
                        new ProjectVersionCreationInfo(settings.VersionName)
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
                    logger.Information($"{settings.VersionName} already exists. Updating it...");
                    logger.Information(
                        $"Updating version with paramenters: {Environment.NewLine}{JsonConvert.SerializeObject(settings)}");
                    await version.Update(settings);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error creating version: {ex.Message}");
            }
        }

        public static async Task MigrateIssuesVersion(MigrateIssuesVersionSettings settings, ICakeLog logger)
        {
            try
            {
                var jira = Atlassian.Jira.Jira.CreateRestClient(settings.Host, settings.User, settings.Password);
                logger.Information($"Finding issues on version {settings.FromVersion}...");
                var issuesToMove =
                    await jira.Issues.GetIssuesFromJqlAsync($"project = {settings.Project} and fixVersion is not EMPTY and fixVersion IN (\"{settings.FromVersion}\")");
                foreach (var issue in issuesToMove)
                {
                    logger.Information($"Moving issue {issue.Key} from {settings.FromVersion} to {settings.ToVersion}");
                    issue.FixVersions.Remove(settings.FromVersion);
                    issue.FixVersions.Add(settings.ToVersion);
                    await jira.Issues.UpdateIssueAsync(issue);
                }
                logger.Information("Issue migration complete!");

            }
            catch (Exception ex)
            {
                logger.Error($"Error migrating issues: {ex.Message}");
                if (ex.InnerException != null)
                {
                    logger.Error(ex.InnerException);
                }
            }
        }
    }
}
