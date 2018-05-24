using System.Threading.Tasks;
using Atlassian.Jira;
using Cake.Jira.Dtos;

namespace Cake.Jira.Helpers
{
    public static class Extensions
    {
        public static async Task Update(this ProjectVersion version, CreateOrUpdateJiraVersionSettings settings)
        {
            version.IsReleased = settings.IsReleased;
            version.Description = settings.Description;
            version.ReleasedDate = settings.ReleaseDate;

            await version.SaveChangesAsync();
        }
    }
}
