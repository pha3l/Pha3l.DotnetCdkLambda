using Amazon.CDK;
using Amazon.CDK.AWS.ECS;
using Constructs;
using Pha3l.DotnetCdkLambda.Cdk.Stacks;

namespace Pha3l.DotnetCdkLambda.Cdk.Stages
{
    public class AppStage : Stage
    {
        public CfnOutput UrlOutput { get; }
        
        public AppStage(Construct scope, string id, StageProps props = null) : base(scope, id, props)
        {
            var service = new ApplicationStack(this, "Application");

            this.UrlOutput = service.UrlOutput;
        }
        
    }
}