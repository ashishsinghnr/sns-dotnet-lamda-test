using System.Threading.Tasks;
using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;
using SnsGatewayLambda;

namespace SnsGatewayLambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestFunctionHandler_MakesHttpCall()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            var snsEvent = new SNSEvent
            {
                Records = new System.Collections.Generic.List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage
                        {
                            Message = "Test message",
                            MessageId = "test-message-id"
                        }
                    }
                }
            };

            var exception = await Record.ExceptionAsync(() => function.FunctionHandler(snsEvent, context));
            Assert.Null(exception);
        }

        [Fact]
        public async Task TestFunctionHandler_WithJsonMessage()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            var jsonMessage = """{"Content":"Test JSON message","EventType":"TestEvent","Timestamp":"2024-01-01T00:00:00Z"}""";
            
            var snsEvent = new SNSEvent
            {
                Records = new System.Collections.Generic.List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage
                        {
                            Message = jsonMessage,
                            MessageId = "test-json-message-id"
                        }
                    }
                }
            };

            var exception = await Record.ExceptionAsync(() => function.FunctionHandler(snsEvent, context));
            Assert.Null(exception);
        }
    }
}