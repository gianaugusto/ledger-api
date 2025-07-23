# Ledger API

A minimal C# web API that manages multiple ledger accounts with in-memory data structures.

## Features

- Create/Use Account by ID
- Record Money Movement per Account
- View Current Balance per Account
- View Transaction History per Account

## Technical Requirements

- C#, .NET 8, ASP.NET Core Minimal API
- In-memory storage: map/dictionary of accountId -> transactions list
- Project structure under src/ folder with unit tests in tests/
- Dockerfile and docker-compose.yml included for easy local deployment

## Project Structure

```
ledger-api/
├── LedgerAPI.sln
├── docker-compose.yml
├── Dockerfile
├── README.md
├── src/
│   └── LedgerAPI/
│       ├── Program.cs
│       ├── Models/
│       └── Services/
└── tests/
    └── LedgerAPI.Tests/
        └── LedgerServiceTests.cs
```

## API Endpoints

### Add Transaction

`POST /accounts/{accountId}/transactions`

Request body:
```json
{
  "type": "deposit" | "withdrawal",
  "amount": decimal,
  "description": string (optional)
}
```

### Get Balance

`GET /accounts/{accountId}/balance`

Returns the balance of the specified account.

### Get Transactions

`GET /accounts/{accountId}/transactions`

Returns all transactions for the specified account.

## Running the Application

### Using Docker

1. Build and run the application with Docker Compose:
   ```
   docker-compose up --build
   ```

2. The API will be available at `http://localhost:5000`

### Running Locally

1. Open the solution in Visual Studio or JetBrains Rider
2. Run the LedgerAPI project
3. The API will be available at `http://localhost:5000`
