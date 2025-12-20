import requests

URL = "http://127.0.0.1:8000/graphql"

def send_request(gql_query):
    """Helper to send GraphQL requests"""
    response = requests.post(URL, json={'query': gql_query})
    if response.status_code == 200:
        return response.json()
    else:
        raise Exception(f"Query failed: {response.status_code}")

def create_user(name, age):
    mutation = f"""
        mutation {{
            createUser(name: "{name}", age: {age}) {{
                id
                name
            }}
        }}
    """
    return send_request(mutation)

def list_users():
    query = """
        query {
            users {
                id
                name
                age
            }
        }
    """
    return send_request(query)

# --- Execution ---
if __name__ == "__main__":
    print("--- 1. Creating Users ---")
    print(create_user("Eve", 28))
    print(create_user("Frank", 50))

    print("\n--- 2. Listing All Users ---")
    response = list_users()
    
    # Pretty print the list
    users = response['data']['users']
    for user in users:
        print(f"User #{user['id']}: {user['name']} ({user['age']} years old)")