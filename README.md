# Haiku.API by Manuel Suarez
### C# | Swagger | XUnit | PostgreSQL (pgAdmin4) | Serilog | .NET | Visual Studio

# Summary
Haiku.API is a robust API that allows you to create and manage haikus and haiku creators, it features Jwt Authentication for secured access, C.R.U.D Operations with built-in validation (Custom Syllable Counter), and Serilog for efficient log tracking and monitoring. 
This project includes comprehensive integration and unit testing to ensure reliability and performance.

# Dependencies
- AutoMapper: A library for object-to-object mapping, commonly used for mapping DTOs to domain models and vice versa.
- BCrypt.Net-Next: A library for hashing passwords using the BCrypt algorithm, providing secure password storage.
- coverlet.collector: A code coverage tool for .NET applications, used to collect coverage data during tests.
- FluentAssertions: A library that provides fluent assertion methods for unit tests, making tests more readable and expressive.
- FluentAssertions.Json: Extends FluentAssertions to include JSON-specific assertions, useful for verifying JSON responses in tests.
- Microsoft.AspNetCore.Authentication.Abstractions: Provides abstractions for authentication in ASP.NET Core applications.
- Microsoft.AspNetCore.Authentication.JwtBearer: Provides JWT Bearer token authentication for securing APIs in ASP.NET Core.
- Microsoft.AspNetCore.Authorization: Provides authorization functionality for ASP.NET Core applications.
- Microsoft.AspNetCore.Mvc.Testing: Simplifies integration testing of ASP.NET Core MVC applications by providing a testable TestServer and WebApplicationFactory.
- Microsoft.Data.Sqlite: A lightweight, in-memory SQLite database provider for .NET applications, useful for testing and development.
- Microsoft.EntityFrameworkCore: A robust Object-Relational Mapper (ORM) for .NET, enabling database access using LINQ queries.
- Microsoft.EntityFrameworkCore.Design: Provides design-time tools for Entity Framework Core, such as migrations and model generation.
- Microsoft.EntityFrameworkCore.Sqlite: The SQLite provider for Entity Framework Core, allowing the use of SQLite as a database.
- Microsoft.EntityFrameworkCore.Tools: Provides command-line tools for Entity Framework Core, including commands for migrations and database updates.
- Microsoft.IdentityModel.JsonWebTokens: Provides support for handling JSON Web Tokens (JWTs) for authentication and authorization.
- Moq: A mocking library for .NET, used for creating mock objects in unit tests.
- Npgsql.EntityFrameworkCore.PostgreSQL: The PostgreSQL provider for Entity Framework Core, allowing the use of PostgreSQL as a database.
- Serilog: A structured logging library for .NET, providing flexible logging to various sinks (e.g., console, files).
- Serilog.Extensions.Logging: Integrates Serilog with ASP.NET Core's logging framework, allowing Serilog to be used as the logging provider.
-Serilog.Sinks.Console: A Serilog sink for logging to the console, useful for debugging and development.
-Serilog.Sinks.Debug: A Serilog sink for logging to the debug output window, useful for development and debugging.
-Serilog.Sinks.File: A Serilog sink for logging to files, allowing for persistent log storage.
-Swashbuckle.AspNetCore: A library for generating Swagger documentation for ASP.NET Core APIs, providing interactive API documentation and testing.
- xunit: A popular testing framework for .NET, used for writing and running unit tests.
- xunit.runner.visualstudio: An xUnit test runner for Visual Studio, enabling test discovery and execution within the IDE.
  
# Instructions
- Create the "myhaikus" database in PostgreSQL (pgAdmin4).
- Run add-migration {anyname} to add a new migration.
- Run update-database to apply the migration.
- Use the following credentials for login management:
  - Username: dev1 | Password: 1234
- Use the generated token in the Authorize window for secured API access.
  
# Features
- User Authentication
- Haiku & Creator Management
- API Implementation
- Integration & Unit Testing
- Comprehensive Logging with Serilog

# Business Rules
- Only development users can access API endpoints.
- Creator Name is required.
- Haiku Title is required.
- All lines of the Haiku are required.
- Haiku must adhere to the correct syllable count (5-7-5).
- CreatorId is required for each Haiku.
  
# Demo
