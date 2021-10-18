using Azure.Messaging.ServiceBus;
//using Microsoft.Azure.ServiceBus;
//using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //can be improved
        private string connectionString = "Endpoint=sb://jamdmicroservicestraining.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jvB1RTl6rmJfs0JJkfQ3RctNQqbC+doRLJYL9Uq7f6s=";

        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            // Using Microsoft.Azure.ServiceBus
            //ISenderClient senderClient = new TopicClient(connectionString, topicName);

            //var jsonMessage = JsonConvert.SerializeObject(message);
            //var finalMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            //{
            //    CorrelationId = Guid.NewGuid().ToString()
            //};

            //await senderClient.SendAsync(finalMessage);
            //await senderClient.CloseAsync();

            // Using Azure.Messaging.ServiceBus

            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
