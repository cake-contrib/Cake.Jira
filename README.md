# Cake.Jira

[![Build status](https://ci.appveyor.com/api/projects/status/od5piwwvn2nyfoet?svg=true)](https://ci.appveyor.com/project/Ninglin/cake-jira)

Cake addin for integration with Jira Issue Tracker

# Using it

In order to use the add-in just reference it as you would any other cake add-in:
```csharp
#addin nuget:?package=Cake.Jira
```
After that you can use the aliases available.

Currently the add-in only supports one alias for creating a version on Jira. Check below for it's usage:

```csharp
Task("Create-Jira-Version")
  .Does(() => {
    CreateJiraVersion(
      new CreateJiraVersionSettings
      {
        Host = "https://your.jira.host.com",
        User = "JustAUser",
        Password = "SuperSecurePassword",
        Project = "ProjectId",
        Description = "Something something bla bla bla lorem freaking ipsum",
        Number = "1.0.0",
        ReleaseDate = DateTime.Now
      }
    );
  });
```

And there you go. You should have a new Jira version created. Keep in mind that if you call ```CreateJiraVersion``` on an existing version it will update it.

# Contributing

This repo follows the [Fork and Pull Request](https://gist.github.com/Chaser324/ce0505fbed06b947d962) standard. You should follow those guidelines in order to contribute.
