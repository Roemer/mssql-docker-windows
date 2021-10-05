#addin nuget:?package=Cake.Docker&version=1.0.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var winVersion = ArgumentOrEnvironmentVariable("winver", String.Empty, "1803");
var sqlServerVersion = ArgumentOrEnvironmentVariable("sqlServerVersion", String.Empty, "2019");

string username = ArgumentOrEnvironmentVariable("dockerhubUsername", String.Empty, String.Empty);
string password = ArgumentOrEnvironmentVariable("dockerhubPassword", String.Empty, String.Empty);
var dockerImageName = ArgumentOrEnvironmentVariable("dockerImageName", String.Empty, $"{username}/mssql-server:{sqlServerVersion}-{winVersion}");
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
   Information($"Building SQL '{sqlVersion}' for Windows '{winVersion}'");
   var edition = "developer";

   DockerBuild(new DockerImageBuildSettings {
      BuildArg = new[] { $"winversion={winVersion}" },
      Tag = new[] { dockerImageName },
      File = @$"{edition}\{sqlVersion}\Dockerfile",
      Isolation = "hyperv",
   }, edition);
      
   
});

Task("Deploy-Images")
.Does(() => {
   var loginSettings = new DockerRegistryLoginSettings();
   loginSettings.username = username;
   loginSettings.password = password;
   DockerLogin(loginSettings);
   DockerPush(dockerImageName);
   DockerLogout();
});


RunTarget(target);