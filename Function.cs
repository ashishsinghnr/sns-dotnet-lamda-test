using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.Core;
using SnsGatewayLambda.Models;
using SnsGatewayLambda.Services;
using SnsGatewayLambda.LambdaHandlerBase;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SnsGatewayLambda
{
    public class Function
    {
        private readonly SNSEventFunction _snsEventFunction;

        public Function()
        {
            _snsEventFunction = new SNSEventFunction();
        }

        public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            await _snsEventFunction.SNSEventFunctionHandler(snsEvent, context);
        }
    }
}