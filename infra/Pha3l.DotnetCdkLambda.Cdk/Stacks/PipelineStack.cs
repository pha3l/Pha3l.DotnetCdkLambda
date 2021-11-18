using Amazon.CDK;
using Amazon.CDK.Pipelines;
using Constructs;

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
                    Input = CodePipelineSource.GitHub("pha3l/Pha3l.DotnetCdkLambda", "main"),
                    Commands = new[]
                    {
                        "dotnet build",
                        "npx cdk synth"
                    }
                })
            });
        }
    }
}