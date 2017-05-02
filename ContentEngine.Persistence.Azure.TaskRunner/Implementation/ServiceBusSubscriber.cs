using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.Azure.TaskRunner.Implementation
{
    class ServiceBusSubscriber : IServiceBusSubscriber
    {
        private readonly IConfigurationRoot _configuration;
        private readonly SubscriptionClient _client;

        public ServiceBusSubscriber(IConfigurationRoot configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration["ConnectionString.AzureServiceBus"];
            _client = new SubscriptionClient(connectionString, "DenormalizeContent", "ContentWriter");
        }

        public void BeginBackgroundProcessing()
        {
            // Configure the callback options.
            MessageHandlerOptions options = new MessageHandlerOptions();
            options.AutoComplete = false;
            options.MaxAutoRenewDuration = TimeSpan.FromMinutes(1);
            options.MaxConcurrentCalls = 4;
            _client.RegisterMessageHandler(ReceiveMessage, options);
        }

        public async Task ReceiveMessage(Message message, CancellationToken token)
        {
            // Process message from subscription.
            Console.WriteLine("\n**Messages**");
            Console.WriteLine("Body: " + message.Body);
            Console.WriteLine("MessageID: " + message.MessageId);
            Console.WriteLine("Sequence Number: " + message.SystemProperties.SequenceNumber);
                
            // Remove message from subscription.
            await _client.CompleteAsync(message.SystemProperties.LockToken);

        }
    }
}
