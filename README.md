# Haiku.API by Manuel Suarez
### C# | Swagger (Swashbuckle) | XUnit | PostgreSQL (pgAdmin4) | Serilog | .NET | RESTful API | Visual Studio

# Summary
Haiku.API is a robust RESTful API that allows you to create and manage haikus and haiku creators, it features Jwt Authentication for secured access, C.R.U.D Operations with built-in validation (Custom Syllable Counter), and Serilog for efficient log tracking and monitoring.

The project includes thorough integration tests for controllers and unit tests for services, with over 20+ tests in total to ensure both reliability and performance.

# Dependencies
- Microsoft.EntityFrameworkCore: ORM for .NET with support for SQLite (Microsoft.EntityFrameworkCore.Sqlite), PostgreSQL (Npgsql.EntityFrameworkCore.PostgreSQL), and design-time tools (Microsoft.EntityFrameworkCore.Design & Microsoft.EntityFrameworkCore.Tools).
- Microsoft.AspNetCore.Authentication: JWT Bearer token authentication (Microsoft.AspNetCore.Authentication.JwtBearer) and abstractions (Microsoft.AspNetCore.Authentication.Abstractions).
- Microsoft.IdentityModel.JsonWebTokens: JSON Web Token handling for authentication.
- Microsoft.AspNetCore.Authorization: Authorization functionality for ASP.NET Core.
- Microsoft.AspNetCore.Mvc.Testing: Simplifies ASP.NET Core MVC integration testing.
- xunit: Unit testing framework with Visual Studio runner (xunit.runner.visualstudio).
- Moq: Mocking library for unit tests.
- FluentAssertions: Fluent assertions for tests, including JSON-specific extensions (FluentAssertions.Json).
- coverlet.collector: Code coverage collector.
- BCrypt.Net-Next: BCrypt algorithm for password hashing.
- AutoMapper: Object-to-object mapping for DTOs and domain models.
- Serilog: Structured logging with various sinks (Console, Debug, File) and ASP.NET Core integration (Serilog.Extensions.Logging).
- Swashbuckle.AspNetCore: Swagger documentation for ASP.NET Core APIs.
- Microsoft.Data.Sqlite: In-memory SQLite provider for development and testing.
  
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
- Only development users can access API endpoints (i.e., requiring login).

# Demo
[![Haiku.API](https://img.youtube.com/vi/p6lQowGQDFQ/0.jpg)](https://www.youtube.com///watch?v=p6lQowGQDFQ "Haiku.API")
