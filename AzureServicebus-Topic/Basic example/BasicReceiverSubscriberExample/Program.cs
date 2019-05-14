using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static ITopicClient topicClient;
        static ISubscriptionClient subscriptionClient;
        static void Main(string[] args)
        {
            string connectionStringServiceBus = "<<connection String Service Bus>>";
            string topicName = "<<Topic Name>>";
            string SubscriptionName = "<<Subscription Name>>";

            subscriptionClient = new SubscriptionClient(connectionStringServiceBus, topicName, SubscriptionName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press any key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            topicClient = new TopicClient(connectionStringServiceBus, topicName);

            RegisterMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            topicClient.CloseAsync().Wait();
        }
        static void RegisterMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Register the function that will process messages
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: Sequence Number:{message.SystemProperties.SequenceNumber} \t Body:{Encoding.UTF8.GetString(message.Body)}");

            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Exception:: {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }
    }
}
