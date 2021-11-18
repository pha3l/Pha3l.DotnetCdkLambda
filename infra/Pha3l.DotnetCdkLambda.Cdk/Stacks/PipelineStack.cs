using Amazon.CDK;
using Amazon.CDK.Pipelines;
using Constructs;
using Pha3l.DotnetCdkLambda.Cdk.Stages;

namespace Pha3l.DotnetCdkLambda.Cdk.Stacks
{
    public class PipelineStack : Stack
    {
        public PipelineStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            var pipeline = new CodePipeline(this, "Pipeline", new CodePipelineProps
            {
                PipelineName = "ApplicationPipeline",
                Synth = new ShellStep("Synth", new ShellStepProps
                {
                    Input = CodePipelineSource.GitHub("pha3l/Pha3l.DotnetCdkLambda", "main", new GitHubSourceOptions
                    {
                        Authentication = SecretValue.SecretsManager("GithubToken")
                    }),
                    Commands = new[]
                    {
                        "dotnet build",
                        "npx cdk synth"
                    }
                })
            });

            pipeline.AddStage(new AppStage(this, "PreProd", new StageProps
            {
                Env = new Environment
                {
                    Account = "004969436191",
                    Region = "us-west-2"
                }
            }));
        }
    }
}