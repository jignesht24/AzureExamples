using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static IQueueClient queueClient;
        static void Main(string[] args)
        {
            string connectionStringServiceBus = "<<Namespace connection string>>";
            string queueName = "<<queue name>>";

            Console.WriteLine("======================================================");
            Console.WriteLine("Press any key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            queueClient = new QueueClient(connectionStringServiceBus, queueName);

            RegisterMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            queueClient.CloseAsync().Wait();
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
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: Sequence Number:{message.SystemProperties.SequenceNumber} \t Body:{Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Exception:: {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }
    }
}
