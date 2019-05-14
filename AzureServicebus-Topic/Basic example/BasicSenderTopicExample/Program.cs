using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicSenderQueueExample
{
    class Program
    {
        static ITopicClient topicClient;
        static void Main(string[] args)
        {
            string connectionStringServiceBus = "<<connection String Service Bus>>";
            string topicName = "<< Topic Name >>";

            SendMessage(connectionStringServiceBus, topicName).GetAwaiter().GetResult();
        }

        static async Task SendMessage(string connectionStringServiceBus, string topicName)
        {
            const int numberOfMessages = 5;
            topicClient = new TopicClient(connectionStringServiceBus, topicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press any key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send Messages
            await SendMessagesToQueueAsync(numberOfMessages);

            Console.ReadKey();

            await topicClient.CloseAsync();
        }

        static async Task SendMessagesToQueueAsync(int numberOfMessages)
        {
            try
            {
                for (var i = 0; i < numberOfMessages; i++)
                {
                    // Message that send to the queue
                    string messageBody = $"GNR-MSG dataid:{i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    Console.WriteLine($"Sending message to queue: {messageBody}");

                    // Send the message to the queue
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }
    }
}
