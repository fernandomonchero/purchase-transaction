# Purchase Transaction API

A RESTful API built with .NET 8 to manage purchase transactions and retrieve historical exchange rates from the U.S. Treasury Fiscal Data API.

# Overview

This project was developed as part of a technical assessment to demonstrate software engineering best practices using the .NET ecosystem.

The API allows clients to:

- Create purchase transactions
- Retrieve transactions by identifier
- Convert transaction values using the historical exchange rate of the purchase date
- Validate business rules before persistence
- Execute unit and integration tests

# Architecture

The solution follows Clean Architecture principles, separating responsibilities into independent layers.

src
├── PurchaseTransaction.Api
├── PurchaseTransaction.Application
├── PurchaseTransaction.Domain
└── PurchaseTransaction.Infrastructure

tests
├── PurchaseTransaction.UnitTests
└── PurchaseTransaction.IntegrationTests

# Layer Responsibilities
| Layer | Responsibility |
| API | HTTP endpoints, dependency injection and middleware configuration |
| Domain | Business rules, entities and validation |
| Infrastructure | Database access, external APIs and persistence |

# Design Decisions

This project intentionally applies several software engineering patterns.

- Dependency Injection for loose coupling
- Repository Pattern to abstract persistence
- Adapter Pattern for communication with external services
- FluentValidation for input validation
- Notification Pattern to collect business validation errors
- SOLID Principles
- Clean Architecture

These choices make the application easier to maintain, test and extend.

# Technologies

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker
- Swagger / OpenAPI
- FluentValidation
- NUnit
- Moq
- SQLite

# Project Structure

PurchaseTransaction
│
├── src
│
├── tests
│
├── docker-compose.yml
│
├── README.md
│
└── LICENSE
	
# Running the Project

Using Docker

docker compose up --build

The API will be available at:

http://localhost:5000

Swagger:

http://localhost:5000/swagger

# Running Locally

Restore packages

dotnet restore

Build

dotnet build

Run the API

dotnet run --project src/PurchaseTransaction.Api

The API will be available at:

http://localhost:5279

Swagger:

http://localhost:5279/swagger

# Running Tests

Execute all tests

dotnet test

# API Endpoints
| Method | Endpoint | Description |
| POST | api/transactions | Creates a purchase transaction |
| GET | api/transactions/{id} | Retrieves a transaction by ID |
| GET | api/transactions | Retrieves all transactions |
| GET | api/countries | Retrieves all countries available in Treasury API until now |
| GET | api/converted-transactions?id={id}&country={country} | Retrieves a transaction by ID, with its value converted to the currency of the country specified |

# External Service

Historical exchange rates are obtained from the U.S. Treasury Fiscal Data API.

To simplify future maintenance, all communication with the external API is isolated behind an Adapter layer.

# Future Improvements

Although the project fulfills the proposed requirements, several improvements could be implemented as the application grows and new requirements arise:

- Authentication and Authorization
- Health Checks
- Exchange Rate Caching
- Rate Limiting
- Structured Logging
- CI/CD Pipeline
- Code Coverage Reports
- API Versioning
- Observability (OpenTelemetry)
