using Azure.Messaging.EventHubs.Consumer;
using System.Text;

namespace IotHubClientConsumerSim
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Client Consumer Sim");

            string eventHubsCompatibleConnectionString = "<built-in-event-hub-connection-string>";
            string eventHubName = "<event-hub-name>";
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            Console.WriteLine("Reading IoT Hub telemetry");

            // Create consumer
            await using var consumer = new EventHubConsumerClient(
                consumerGroup,
                eventHubsCompatibleConnectionString,
                eventHubName
            );

            // Receive loop
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync())
            {
                string data = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                Console.WriteLine($"[Received] Partition: {partitionEvent.Partition.PartitionId}, Data: {data}");
            }
        }
    }
}