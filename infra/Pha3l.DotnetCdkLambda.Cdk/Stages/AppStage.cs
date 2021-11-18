using Amazon.CDK;
using Amazon.CDK.AWS.ECS;
using Constructs;
using Pha3l.DotnetCdkLambda.Cdk.Stacks;

namespace Pha3l.DotnetCdkLambda.Cdk.Stages
{
    public class AppStage : Stage
    {
        public CfnOutput UrlOutput1 { get; }
        public CfnOutput UrlOutput2 { get; }
        
        public AppStage(Construct scope, string id, StageProps props = null) : base(scope, id, props)
        {
            var service = new ApplicationStack(this, "Application");
            var service2 = new ApplicationStackTwo(this, "ApplicationTwo");

            this.UrlOutput1 = service.UrlOutput;
            this.UrlOutput2 = service2.UrlOutput;
        }
        
    }
}