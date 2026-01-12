import redis
import time
import sys

# 1. Connect to Redis
# decode_responses=True ensures we get strings back, not bytes
r = redis.Redis(host='localhost', port=6379, decode_responses=True)

def fetch_from_slow_database(product_id):
    """Simulates a slow database query"""
    time.sleep(2)  # Simulate 2-second delay
    
    mock_db = {
        "101": "iPhone 15 Pro",
        "102": "Samsung Galaxy S24",
        "103": "Sony Headphones"
    }
    return mock_db.get(product_id)

def main():
    print("--- Redis Cache-Aside Demo (Python) ---")
    print("Enter a Product ID (e.g., 101). Type 'exit' to quit.\n")

    while True:
        product_id = input("Enter Product ID: ")
        if product_id.lower() == 'exit':
            break

        cache_key = f"product:{product_id}"
        
        # Start timing
        start_time = time.time()

        # STEP A: Check Redis Cache First
        cached_value = r.get(cache_key)

        if cached_value:
            # HIT! Found in cache
            elapsed = (time.time() - start_time) * 1000
            print(f"\033[92m[CACHE HIT] Found: {cached_value}")
            print(f"Time Taken: {elapsed:.2f}ms (Super Fast!)\033[0m\n")
        else:
            # MISS! Not in cache
            print("\033[93m[CACHE MISS] Not found in Redis. Fetching from Slow Database...\033[0m")
            
            db_value = fetch_from_slow_database(product_id)

            if db_value:
                # STEP B: Save to Redis (Expires in 60 seconds)
                # setex = Set with Expiration
                r.setex(cache_key, 60, db_value)

                elapsed = (time.time() - start_time) * 1000
                print(f"[DB READ] Found: {db_value}")
                print(f"Time Taken: {elapsed:.2f}ms (Slow...)\n")
            else:
                print("Product does not exist in Database.\n")

if __name__ == "__main__":
    main()