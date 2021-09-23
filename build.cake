#addin nuget:?package=Cake.Docker&version=1.0.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

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
   var winVersions = new[] { "1803", "1809", "1903", "1909", "2004", "20H2", "21H1", "21H2", "ltsc2016", "ltsc2019", "ltsc2022" };

   foreach (var winVersion in winVersions) {
      Information(winVersion);
   }

   /*DockerBuild(new DockerImageBuildSettings {
      BuildArg = new[] { "" },
      Tag = new[] { "test" },
      Network = "Default Switch"
   }, "developer");*/
});

RunTarget(target);
