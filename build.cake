#addin nuget:?package=Cake.Docker&version=1.0.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var winVersions = Argument("winversion", "1803;1809;1903").Split(';');
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

   var sqlServerVersions = new[] {
      //"2016_SP2",
      //"2016_SP3",
      "2019",
   };

   foreach (var sqlVersion in sqlServerVersions) {
      foreach (var winVersion in winVersions) {
         Information($"Building SQL '{sqlVersion}' for Windows '{winVersion}'");
         var tag = $"mssql-server:{sqlVersion}-{winVersion}";
         var edition = "developer";

         DockerBuild(new DockerImageBuildSettings {
            BuildArg = new[] { $"winversion={winVersion}" },
            Tag = new[] { tag },
            File = @$"{edition}\{sqlVersion}\Dockerfile",
            Isolation = "hyperv",
         }, edition);
      }
   }
});

RunTarget(target);