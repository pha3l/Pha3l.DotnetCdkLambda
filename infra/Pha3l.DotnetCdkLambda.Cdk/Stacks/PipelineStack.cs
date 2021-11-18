using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.Pipelines;
using Constructs;
using Pha3l.DotnetCdkLambda.Cdk.Stages;

namespace Pha3l.DotnetCdkLambda.Cdk.Stacks
{
    public class PipelineStack : Stack
    {
        public PipelineStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            var input = CodePipelineSource.GitHub("pha3l/Pha3l.DotnetCdkLambda", "main", new GitHubSourceOptions
            {
                Authentication = SecretValue.SecretsManager("GithubToken")
            });
            
            var pipeline = new CodePipeline(this, "Pipeline", new CodePipelineProps
            {
                CrossAccountKeys = true,
                PipelineName = "ApplicationPipeline",
                Synth = new CodeBuildStep("Synth", new CodeBuildStepProps
                {
                    Input = input,
                    Commands = new[]
                    {
                        "dotnet build",
                        "npx cdk synth"
                    },
                    BuildEnvironment = new BuildEnvironment
                    {
                        Privileged = true,
                        BuildImage = LinuxBuildImage.STANDARD_5_0,
                        ComputeType = ComputeType.SMALL
                    }
                })
            });

            var preprod = new AppStage(this, "PreProd", new StageProps
            {
                Env = new Environment
                {
                    Account = "004969436191",
                    Region = "us-west-2"
                }
            });

            var reportGroup = new ReportGroup(this, "IntegrationReports", new ReportGroupProps
            {
                ReportGroupName = "IntegrationReports"
            });

            var reports = new Dictionary<string, object>
            {
                {
                    "reports", new Dictionary<string, object>
                    {
                        {
                            reportGroup.ReportGroupArn, new Dictionary<string, object>
                            {
                                { "file-format", "VisualStudioTrx" },
                                { "files", "**/*" },
                                { "base-directory", "test/Pha3l.DotnetCdkLambda.IntegrationTests/TestResults" }
                            }
                        }
                    }
                }
            };

            var preprodStage = pipeline.AddStage(preprod, new AddStageOpts
            {
                Post = new Step[]
                {
                    new CodeBuildStep("IntegrationTests", new CodeBuildStepProps
                    {
                        Input = input,
                        InstallCommands = new[]
                        {
                            "dotnet tool install -g trx2junit",
                            "export PATH=\"$PATH:/root/.dotnet/tools\""
                        },
                        Commands = new[]
                        {
                            "dotnet build",
                            "dotnet test test/Pha3l.DotnetCdkLambda.IntegrationTests --logger \"trx;LogFileName=TestResults.trx\""
                        },
                        BuildEnvironment = new BuildEnvironment
                        {
                            BuildImage = LinuxBuildImage.STANDARD_5_0
                        },
                        PartialBuildSpec = BuildSpec.FromObject(reports),
                        RolePolicyStatements = new[]
                        {
                            new PolicyStatement(new PolicyStatementProps
                            {
                                Actions = new string[]
                                {
                                    "codebuild:CreateReportGroup",
                                    "codebuild:CreateReport",
                                    "codebuild:UpdateReport",
                                    "codebuild:BatchPutTestCases",
                                    "codebuild:BatchPutCodeCoverages"
                                },
                                Effect = Effect.ALLOW,
                                Resources = new string[] { reportGroup.ReportGroupArn }
                            }),
                        },
                        Env = new Dictionary<string, string>
                        {
                            { "ENDPOINT", "https://esne6g6ty0.execute-api.us-west-2.amazonaws.com/prod/" }
                        }
                    })
                }
            });

            // pipeline.AddStage(new AppStage(this, "Prod", new StageProps
            // {
            //     Env = new Environment
            //     {
            //         Account = "004969436191",
            //         Region = "us-east-1"
            //     }
            // }));
        }
    }
}