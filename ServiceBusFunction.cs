using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ServiceBusBatching
{
    public class ServiceBusFunction
    {
        [FunctionName("ServiceBusFunction")]
        public async Task RunAsync([ServiceBusTrigger("vbonnerqueue", Connection = "MyConStr", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage[] messages,
        ServiceBusMessageActions messageActions,
        ILogger log)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                ServiceBusReceivedMessage message = messages[i];

                try
                {
                    if(i == messages.Length - 1)
                    {
                        // Simulate exception
                        throw new Exception("Simulated exception");
                    }

                    // Process the message
                    string messageBody = message.Body.ToString();
                    log.LogInformation($"Processing message: {messageBody}");

                    // Simulate message processing
                    await Task.Delay(100);

                    // Mark message as completed
                    await messageActions.CompleteMessageAsync(message);
                    log.LogInformation($"Message completed: {messageBody}");
                }
                catch (Exception ex)
                {
                    log.LogError($"Exception occurred while processing message: {ex.Message}");

                    if(message.DeliveryCount >= 5)
                    {
                        // Mark message as dead-lettered
                        await messageActions.DeadLetterMessageAsync(message);
                        log.LogInformation($"Message dead-lettered: {message.Body}");
                    }
                    else
                    {
                        // Mark message as failed
                        await messageActions.AbandonMessageAsync(message);
                        log.LogInformation($"Message abandoned: {message.Body}");
                    }
                }
            }
        }
    }
}
