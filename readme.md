# Service Bus Function using Batching and AutoCompleteMessage disabled.


[Azure Service Bus trigger for Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=python-v2%2Cin-process%2Cnodejs-v4%2Cextensionv5&pivots=programming-language-csharp#attributes) allows you the option to disable AutoCompleteMessage. Which lets the developer handle each message completion within code. Which can be useful in scenarios where you want to execute messages in batches, but don't want to re-try the entire batch for a single message failure.

By default the Function uses Batches with a maxMessageBatchSize of 1000 - [Azure Service Bus bindings for Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus?tabs=isolated-process%2Cextensionv5%2Cextensionv3&pivots=programming-language-csharp#hostjson-settings)

You can view the data for each message, including deliveryCount from the [ServiceBusReceivedMessage](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusreceivedmessage?view=azure-dotnet).

For each message, you will need to manually complete the action for it using [ServiceBusMessageActions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.webjobs.servicebus.servicebusmessageactions?view=azure-dotnet).
