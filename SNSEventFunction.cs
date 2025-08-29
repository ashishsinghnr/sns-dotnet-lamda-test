using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using SnsGatewayLambda.Models;
using SnsGatewayLambda.LambdaHandlerBase;

namespace SnsGatewayLambda.Services
{
    public class SNSEventFunction : SNSFunctionBase<EventMessage>
    {
        public new async Task SNSEventFunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            foreach (var record in snsEvent.Records)
            {
                var message = record.Sns.Message;
                var messagePayload = DeserializeMessage(message);
                await ProcessMessageAsync(messagePayload, context);
            }
        }

        protected override EventMessage DeserializeMessage(string message)
        {
            try
            {
                // Try to deserialize as EventMessage first
                return JsonSerializer.Deserialize<EventMessage>(message) ?? new EventMessage { Content = message };
            }
            catch
            {
                // If deserialization fails, treat as plain text message
                return new EventMessage 
                { 
                    Content = message,
                    Timestamp = DateTime.UtcNow,
                    EventType = "PlainText"
                };
            }
        }

        protected override async Task ProcessMessageAsync(EventMessage messagePayload, ILambdaContext context)
        {
            context.Logger.LogLine($"Processing message: {messagePayload.Content}");
            context.Logger.LogLine($"Event Type: {messagePayload.EventType}");
            context.Logger.LogLine($"Timestamp: {messagePayload.Timestamp}");

            try
            {
                var response = await httpClient.GetAsync("https://httpbin.org/get");
                var responseBody = await response.Content.ReadAsStringAsync();
                context.Logger.LogLine($"httpbin.org response: {responseBody}");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"HTTP call failed: {ex.Message}");
                throw; // Re-throw to be handled by the base class
            }
        }
    }
}