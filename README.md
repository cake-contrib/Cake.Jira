# Cake.Jira

[![NuGet](https://img.shields.io/nuget/v/Cake.Jira.svg?style=flat-square)](https://www.nuget.org/packages/Cake.Jira/) [![Build status](https://ci.appveyor.com/api/projects/status/od5piwwvn2nyfoet?svg=true)](https://ci.appveyor.com/project/Ninglin/cake-jira)

Cake addin for integration with Jira Issue Tracker

# Using it

In order to use the add-in just reference it as you would any other cake add-in:
```csharp
#addin nuget:?package=Cake.Jira
```
After that you can use the aliases available.

Currently the add-in supports: 

* One alias for creating a version on Jira. 

**Usage:**
```csharp
Task("Create-Jira-Version")
  .Does(async () => {
    await CreateOrUpdateJiraVersion(
      new CreateOrUpdateJiraVersionSettings
      {
        Host = "https://your.jira.host.com",
        User = "JustAUser",
        Password = "SuperSecurePassword",
        Project = "ProjectKey",
        Description = "Something something bla bla bla lorem freaking ipsum",
        VersionName = "1.0.0",
        ReleaseDate = DateTime.Now
      }
    );
  });
```

* One alias for moving all issues from one version to another:

**Usage:**
```csharp
Task("Migrate-Issues-To-Version")
	.Does(async () => {
		await MigrateIssuesVersion(new MigrateIssuesVersionSettings{
			Host = "https://your.jira.host.com",
			User = "JustAUser",
			Password = "SuperSecurePassword",
			Project = "ProjectKey",
			FromVersion = "AnOldVersion",
			ToVersion = "ABrandNewVersion",
		});
	});
```

# Contributing

This repo follows the [Fork and Pull Request](https://gist.github.com/Chaser324/ce0505fbed06b947d962) standard. You should follow those guidelines in order to contribute.
