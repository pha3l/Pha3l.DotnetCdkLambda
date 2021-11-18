using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Pha3l.DotnetCdkLambda.FunctionTwo
{
    public class Function
    {
        
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                Body = "Hello from a different lambda function!",
                StatusCode = 200
            };
        }
    }
}
