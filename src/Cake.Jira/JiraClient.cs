using System;
using System.Globalization;
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
                var jira = CreateJira(settings);
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
                var jira = CreateJira(settings);
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

        public static async Task CreateJiraIssue(CreateIssueSettings settings, ICakeLog logger)
        {
            try
            {
                logger.Information($"Creating jira issue '{settings.Summary}' on {settings.Project}");

                var jira = CreateJira(settings);
                var issueSettings = new CreateIssueFields(settings.Project);

                var issue = new Issue(jira, issueSettings)
                {
                    Reporter = settings.Reporter,
                    Summary = settings.Summary,
                    Description = settings.Description,
                    Environment = settings.Environment,
                    Assignee = settings.Assignee,
                    DueDate = settings.DueDate
                };

                if (settings.Priority != 0)
                {
                    issue.Priority = new IssuePriority(settings.Priority.ToString(CultureInfo.InvariantCulture));
                }

                if (settings.Type != 0)
                {
                    issue.Type = new IssueType(settings.Type.ToString(CultureInfo.InvariantCulture));
                }

                if (settings.Labels != null)
                {
                    issue.Labels.AddRange(settings.Labels);
                }

                var createdIssueKey = await issue.Jira.Issues.CreateIssueAsync(issue);

                if (!string.IsNullOrEmpty(createdIssueKey))
                {
                   logger.Information($"Jira issue '{settings.Summary}' created with key: " + createdIssueKey);
                }
                else
                {
                   logger.Information($"Unable to create jira issue '{settings.Summary}'");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error creating issue '{settings.Summary}': {ex.Message}");
            }
        }

        private static Atlassian.Jira.Jira CreateJira(JiraSettings settings)
        {
            return Atlassian.Jira.Jira.CreateRestClient(settings.Host, settings.User, settings.Password);
        }
    }
}
