using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using NewRelic.Api.Agent;

namespace SnsGatewayLambda.LambdaHandlerBase
{
    public abstract class SNSFunctionBase<TMessagePayload>
    {
        protected static readonly HttpClient httpClient = new HttpClient();

        [Transaction]
        public async Task SNSEventFunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            foreach (var record in snsEvent.Records)
            {
                try
                {
                    context.Logger.LogLine($"Processing SNS record: {record.Sns.MessageId}");
                    
                    // Deserialize the message payload
                    var messagePayload = DeserializeMessage(record.Sns.Message);
                    
                    // Process the message using the derived class implementation
                    await ProcessMessageAsync(messagePayload, context);
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine($"Error processing SNS record {record.Sns.MessageId}: {ex.Message}");
                }
            }
        }

        protected abstract TMessagePayload DeserializeMessage(string message);
        
        protected abstract Task ProcessMessageAsync(TMessagePayload messagePayload, ILambdaContext context);
    }
}