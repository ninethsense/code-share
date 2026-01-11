using Confluent.Kafka;


class Program
{
    static async Task Main(string[] args)
    {
        // 1. Configuration
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        // 2. Create the Producer
        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            Console.WriteLine("Kafka Producer Connected. Type a message and press Enter (or 'exit' to quit):");

            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "exit") break;

                try
                {
                    // 3. Send the Message to "test-topic"
                    var deliveryResult = await producer.ProduceAsync(
                        "test-topic",
                        new Message<Null, string> { Value = input }
                    );

                    Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}