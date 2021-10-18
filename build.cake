#addin nuget:?package=Cake.Docker&version=1.0.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var winVersion = EnvironmentVariable<string>("winver", "1803");
var sqlServerVersion = EnvironmentVariable<string>("sqlServerVersion", "2019");

string username = EnvironmentVariable<string>("dockerhubUsername", String.Empty);
var dockerImageName = EnvironmentVariable<string>("dockerImageName", $"{username}/mssql-server:{sqlServerVersion}-{winVersion}");
/*
Windows versions:
      "1803",
      "1809",
      "1903",
      "1909",
      "2004",
      "20H2",
      "21H1",
      "21H2",
      "ltsc2016",
      "ltsc2019",
      "ltsc2022"
*/

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   Information("Running tasks...");
});

Teardown(ctx =>
{
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

Task("Build-Images")
.Does(() => {
   Information($"Building SQL '{sqlServerVersion}' for Windows '{winVersion}'");
   var edition = "developer";

   DockerBuild(new DockerImageBuildSettings {
      BuildArg = new[] { $"winversion={winVersion}" },
      Tag = new[] { dockerImageName },
      File = @$"{edition}\{sqlServerVersion}\Dockerfile",
      Isolation = "hyperv",
   }, edition);
      
   
});

Task("Deploy-Images")
.Does(() => {
   var loginSettings = new DockerRegistryLoginSettings();
   loginSettings.Username = username;
   loginSettings.PasswordStdin = true;
   DockerLogin(loginSettings);
   DockerPush(dockerImageName);
   DockerLogout();
});


RunTarget(target);