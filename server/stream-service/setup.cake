#load nuget:?package=ce.devops.scripts.build.cake

Environment.SetupDefaultEnvironmentVariables(Context);

//NOTE: To modify specific environment variables
//SonarQubeSetup.SetEnvironmentVariableNames(Context, projectKeyVariable: "SONAR_PROJECTKEY");

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "IotStreamApp.IotStreamService",
                            enablePackAndPublishSeparation: true);

TestingSettings.Setup(context: Context);

//ToolSettings.SetToolSettings(context: Context);

//NOTE: To set extra SonarQube parameters such as coverageExclusions, uncomment the following line
//SonarQubeSetup.SetSonarQubeParameters(coverageExclusions: "**/wwwroot/**.*,**/Migrations/**.*");

//NOTE: To add a step before another one, use the following as an example
// BuildParameters.Tasks.DotNetCoreBuildTask.IsDependentOn("CustomTask");
// Task("CustomTask").Does(()=>{
//     Information("CustomTask - Executed before DotNetCoreBuild Task");
// });

Build.RunDotNetCore();