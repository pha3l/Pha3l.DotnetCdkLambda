using Amazon.CDK;
using Pha3l.DotnetCdkLambda.Cdk.Stacks;

namespace Pha3l.DotnetCdkLambda.Cdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new PipelineStack(app, "PipelineStack", new StackProps
            {
                Env = new Environment
                {
                    Account = "004969436191",
                    Region = "us-west-2"
                }
            });
            
            app.Synth();
        }
    }
}
