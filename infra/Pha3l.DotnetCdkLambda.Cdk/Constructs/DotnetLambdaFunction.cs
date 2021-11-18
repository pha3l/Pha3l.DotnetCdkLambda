using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace Pha3l.DotnetCdkLambda.Cdk.Constructs
{
    public class DotnetLambdaFunctionProps : FunctionProps
    {
        public string Project { get; set; }

        public new Code Code => Code.FromAsset("../../src", new AssetOptions
        {
            Bundling = new BundlingOptions
            {
                Command = BundlingCommand(Project),
                Image = Runtime.DOTNET_CORE_3_1.BundlingImage,
                User = "root"
            }
        });

        private static string[] BundlingCommand(string project) => new[]
        {
            "/bin/sh",
            "-c",
            $"dotnet lambda package --project-location {project} " +
            $"&& cp /asset-input/{project}/bin/Release/netcoreapp3.1/{project}.zip /asset-output/"
        };
    }

    public class DotnetLambdaFunction : Function
    {
        // public Function Function { get; }

        public DotnetLambdaFunction(Construct scope, string id, DotnetLambdaFunctionProps props) :
            base(scope, id, props)
        {
        }
    }
}