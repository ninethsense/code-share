using Confluent.Kafka;

class Program
{
    static void Main(string[] args)
    {
        // 1. Configuration
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "my-csharp-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest // Start from beginning if no offset exists
        };

        // 2. Create the Consumer
        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            // 3. Subscribe to the topic
            consumer.Subscribe("test-topic");

            Console.WriteLine("Listening for messages... (Press Ctrl+C to quit)");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    // 4. Consume loop
                    try
                    {
                        // Wait for a message
                        var consumeResult = consumer.Consume(cts.Token);
                        Console.WriteLine($"Received: '{consumeResult.Message.Value}' from Partition: {consumeResult.Partition}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                consumer.Close();
            }
        }
    }
}