using StackExchange.Redis;
using System.Diagnostics; // To measure speed

class Program
{
    // 1. Connection to Redis
    // "lazy" means we only connect when we actually need it.
    private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
    {
        return ConnectionMultiplexer.Connect("localhost:6379");
    });

    public static ConnectionMultiplexer Connection => lazyConnection.Value;

    static async Task Main(string[] args)
    {
        var db = Connection.GetDatabase();

        Console.WriteLine("--- Redis Cache-Aside Demo ---");
        Console.WriteLine("Enter a Product ID (e.g., 101) to fetch details.");
        Console.WriteLine("Type 'exit' to quit.\n");

        while (true)
        {
            Console.Write("Enter Product ID: ");
            string productId = Console.ReadLine();
            if (productId == "exit") break;

            string cacheKey = $"product:{productId}";
            Stopwatch sw = Stopwatch.StartNew();

            // STEP A: Check Redis Cache First
            string cachedValue = await db.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedValue))
            {
                // HIT! Found in cache
                sw.Stop();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[CACHE HIT] Found: {cachedValue}");
                Console.WriteLine($"Time Taken: {sw.ElapsedMilliseconds}ms (Super Fast!)\n");
                Console.ResetColor();
            }
            else
            {
                // MISS! Not in cache, go to "Database"
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[CACHE MISS] Not found in Redis. Fetching from Slow Database...");

                string dbValue = FetchFromSlowDatabase(productId); // This takes 2 seconds

                if (dbValue != null)
                {
                    // STEP B: Save to Redis for next time (Expires in 60 seconds)
                    await db.StringSetAsync(cacheKey, dbValue, TimeSpan.FromSeconds(60));

                    sw.Stop();
                    Console.WriteLine($"[DB READ] Found: {dbValue}");
                    Console.WriteLine($"Time Taken: {sw.ElapsedMilliseconds}ms (Slow...)\n");
                }
                else
                {
                    Console.WriteLine("Product does not exist in Database.\n");
                }
                Console.ResetColor();
            }
        }
    }

    // SIMULATED DATABASE
    // In a real app, this would be your SQLite/SQL Server call.
    static string FetchFromSlowDatabase(string id)
    {
        // Simulate a heavy query delay
        Thread.Sleep(2000);

        // Simple mock data
        return id switch
        {
            "101" => "iPhone 15 Pro",
            "102" => "Samsung Galaxy S24",
            "103" => "Sony Headphones",
            _ => null
        };
    }
}