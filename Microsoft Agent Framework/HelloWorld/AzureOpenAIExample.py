# pip install agent-framework python-dotenv

import asyncio
import os
from dotenv import load_dotenv
from agent_framework.azure import AzureOpenAIChatClient

# Load environment variables from .env file
load_dotenv()

api_key = os.getenv("AZURE_OPENAI_API_KEY")
deployment_name = os.getenv("AZURE_OPENAI_DEPLOYMENT")
endpoint = os.getenv("AZURE_OPENAI_ENDPOINT")
api_version = os.getenv("AZURE_OPENAI_API_VERSION")
agent = AzureOpenAIChatClient(
        endpoint=endpoint, 
        api_key = api_key,
        deployment_name=deployment_name,
        api_version=api_version
    ).create_agent(
        instructions="You are a poet",
        name="Poet"
)

async def main():
    result = await agent.run("Write a two liner poem on nature")
    print(result.text)

asyncio.run(main())