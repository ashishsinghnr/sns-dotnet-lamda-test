# SNS Gateway Lambda (.NET 8)

## Overview
This project implements an AWS Lambda function in .NET 8 that acts as an SNS gateway. It processes SNS messages, makes an HTTP GET request to httpbin.org for each message, and logs the results.

## Implementation Details
- `Function.cs`: Lambda entry point, delegates to `SNSEventFunction`.
- `SNSEventFunction.cs`: Main handler logic, processes SNS events and makes HTTP calls.
- `SnsFunctionBase.cs`: Abstract base class for generic SNS event handling.
- `EventMessage.cs`: Model for deserialized SNS message payloads.
- `SnsGatewayLambda.csproj`: Project file.
- **Language/Runtime:** .NET 8 (C#)
- **Trigger:** AWS SNS Topic
- .NET 8 SDK (macOS ARM64 supported)
- AWS CLI configured with credentials
- AWS Lambda Tools for .NET (`dotnet tool install -g Amazon.Lambda.Tools`)
- An existing SNS topic in AWS
- **HTTP Call:** Uses `HttpClient` to call `https://httpbin.org/get` for each SNS message received.
- **Logging:** Logs SNS message and HTTP response using `context.Logger.LogLine`.
- **IAM Role:** Requires `AWSLambdaBasicExecutionRole` for CloudWatch logging.
- **Deployment:**
  - Build and publish with `dotnet publish -c Release -r linux-x64 --self-contained false`
  - Zip the publish output and deploy using AWS CLI
  - Subscribe Lambda to SNS topic and add invoke permissions

## Design
- **Function.cs:**
  - Static handler method processes all SNS records in the event.
  - For each record, logs the message and makes an HTTP GET request.
  - Logs the HTTP response for traceability.
- **Test:**
  - Simple xUnit test to verify handler logic (no assertion, just execution).
- **CloudWatch:**
  - All logs are available in `/aws/lambda/<function-name>` log group.


## SNS Trigger Setup
1. **Subscribe Lambda to SNS Topic:**
   ```sh
   aws sns subscribe \
     --topic-arn arn:aws:sns:us-west-2:466768951184:ProsperOps-EC2-Instance-State-Changes \
     --protocol lambda \
     --notification-endpoint arn:aws:lambda:us-west-2:466768951184:function:ashish-sns-dotnet-lambda \
     --region us-west-2
   ```
2. **Allow SNS to Invoke Lambda:**
   ```sh
     --principal sns.amazonaws.com \
     --source-arn arn:aws:sns:us-west-2:466768951184:ProsperOps-EC2-Instance-State-Changes \
     --region us-west-2
   ```

## Testing Invocation
To test the Lambda and SNS integration, publish messages to the SNS topic:
```sh
for i in {1..20}; do \
    --message "{\"test\":\"SNS to Lambda .NET 8 - $i\"}" \
    --region us-west-2; \

## CloudWatch Log Verification
Check AWS CloudWatch Logs for `/aws/lambda/ashish-sns-dotnet-lambda` to verify:
- SNS message received
- HTTP call to httpbin.org and response

## Example Log Output
```
Received SNS message: SNS to Lambda .NET 8 - 1
httpbin.org response: { ... }
```

## Notes
- Ensure the Lambda execution role has the correct permissions.
- The function is stateless and can handle multiple SNS records per invocation.
- For load testing, publish multiple SNS messages in a loop.
# sns-dotnet-lamda-test
