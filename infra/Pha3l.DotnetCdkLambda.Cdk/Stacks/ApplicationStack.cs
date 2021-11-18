using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Pha3l.DotnetCdkLambda.Cdk.Constructs;

namespace Pha3l.DotnetCdkLambda.Cdk.Stacks
{
    public class ApplicationStack : Stack
    {
        public CfnOutput UrlOutput { get; }
        internal ApplicationStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var functionOne = new DotnetLambdaFunction(this, "FunctionOne", new DotnetLambdaFunctionProps
            {
                Project = "Pha3l.DotnetCdkLambda.FunctionOne",
                Handler = "Pha3l.DotnetCdkLambda.FunctionOne::Pha3l.DotnetCdkLambda.FunctionOne.Function::FunctionHandler",
                MemorySize = 256
            });

            var api = new LambdaRestApi(this, "Api", new LambdaRestApiProps
            {
                Handler = functionOne.Function
            });

            this.UrlOutput = new CfnOutput(this, "Url", new CfnOutputProps
            {
                Value = api.Url
            });
        }
    }
}
