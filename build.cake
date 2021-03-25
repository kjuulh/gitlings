var target = Argument("target", "Copy Files");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory($"./src/GitLings/GitLings/bin/{configuration}");
    CleanDirectory($"./bin");
});

Task("Build Linux")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCorePublish("./src/GitLings/GitLings.sln", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        PublishSingleFile = true,
        SelfContained = true,
      //  Runtime = "win-x86"
        Runtime = "linux-x64"
    });
});

Task("Build Windows")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCorePublish("./src/GitLings/GitLings.sln", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        PublishSingleFile = true,
        SelfContained = true,
        Runtime = "win-x86"
    });
});

Task("Build")
    .IsDependentOn("Build Linux")
    .IsDependentOn("Build Windows");

Task("Copy Files")
    .IsDependentOn("Build")
    .Does(() => 
{
    CreateDirectory("./bin/linux-x64");
    CopyFiles($"./src/GitLings/GitLings/bin/{configuration}/net5.0/linux-x64/publish/*", "./bin/linux-x64");

    CreateDirectory("./bin/win-x86");
    CopyFiles($"./src/GitLings/GitLings/bin/{configuration}/net5.0/win-x86/publish/*", "./bin/win-x86");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
