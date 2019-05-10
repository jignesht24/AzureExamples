using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicSenderQueueExample
{
    class Program
    {
        static IQueueClient queueClient;
        static void Main(string[] args)
        {
            string connectionStringServiceBus = "<<Namespace connection string>>";
            string queueName = "<<queue name>>";

            SendMessage(connectionStringServiceBus, queueName).GetAwaiter().GetResult();
        }

        static async Task SendMessage(string connectionStringServiceBus, string queueName)
        {
            const int numberOfMessages = 5;
            queueClient = new QueueClient(connectionStringServiceBus, queueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press any key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send Messages
            await SendMessagesToQueueAsync(numberOfMessages);

            Console.ReadKey();

            await queueClient.CloseAsync();
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
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }
    }
}
