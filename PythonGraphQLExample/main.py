# Import required libraries for GraphQL, FastAPI, and database operations
import strawberry
from fastapi import FastAPI
from sqlmodel import Field, SQLModel, create_engine, Session, select
from strawberry.fastapi import GraphQLRouter

# Step A. Database Setup (SQLModel) 
# Define the Database Table for "code first" approach
# This class represents the User table with id, name, and age columns
class UserDB(SQLModel, table=True):
    id: int | None = Field(default=None, primary_key=True)
    name: str
    age: int

# Create the Database Engine
# Using SQLite for simplicity
# This establishes a connection to the SQLite database file
engine = create_engine("sqlite:///database.db")

# Create tables and seed data on startup
# This function initializes the database schema and adds sample data if the table is empty
def create_db_and_tables():
    # Create all database tables based on SQLModel definitions
    SQLModel.metadata.create_all(engine)
    # Open a database session to perform operations
    with Session(engine) as session:
        # Check if empty, then add data
        if not session.exec(select(UserDB)).first():
            # Add sample user records to the database
            session.add(UserDB(name="Praveen Nair", age=30))
            session.add(UserDB(name="Ambika", age=25))
            # Commit the transaction to save the data
            session.commit()

# Step B. Strawberry GraphQL Setup 
# Define the GraphQL Type (What the client sees)
# This class defines the structure of User data exposed via GraphQL API
@strawberry.type
class UserType:
    id: int
    name: str
    age: int

# Define the Logic / Resolvers to fetch data from DB
# This function queries the database and returns all users as GraphQL types
def get_users():
    # Open a database session for querying
    with Session(engine) as session:
        # Build a SELECT query for all users
        statement = select(UserDB)
        # Execute the query and fetch all results
        results = session.exec(statement).all()
        # Convert DB models to GraphQL types
        return [UserType(id=u.id, name=u.name, age=u.age) for u in results]

# Define the root GraphQL Query type
@strawberry.type
class Query:
    @strawberry.field
    def users(self) -> list[UserType]:
        return get_users()

@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_user(self, name: str, age: int) -> UserType:
        with Session(engine) as session:
            # 1. Create the DB Object
            db_user = UserDB(name=name, age=age)
            
            # 2. Add to DB and Commit
            session.add(db_user)
            session.commit()
            
            # 3. Refresh to get the generated ID
            session.refresh(db_user)
            
            # 4. Return the API Type
            return UserType(id=db_user.id, name=db_user.name, age=db_user.age)

# Create the GraphQL schema with the Query type
schema = strawberry.Schema(query=Query, mutation=Mutation)

# Step C. FastAPI App 
# Run DB setup before app starts
# Initialize the database and seed data on application startup
create_db_and_tables()

# Create a GraphQL router with the defined schema
graphql_app = GraphQLRouter(schema)
# Create the FastAPI application instance
app = FastAPI()
# Mount the GraphQL router at the /graphql endpoint
app.include_router(graphql_app, prefix="/graphql")

