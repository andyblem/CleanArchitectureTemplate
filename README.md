# Clean Architecture Template

A modern, production-ready .NET 9 Web API template built with Clean Architecture principles, featuring JWT authentication, Entity Framework Core auditing, and comprehensive soft delete functionality. The backend is .NET WebAPI and the frontend is Angular.

## 🏗️ Architecture Overview

This template follows Clean Architecture patterns with clear separation of concerns across multiple layers:

```
┌─────────────────────────────────────────────────────────┐
│                  Presentation Layer                     │
│  ┌─────────────────────┐  ┌─────────────────────────┐   │
│  │   Web API           │  │   Angular Client        │   │
│  │   (Controllers)     │  │   (Frontend)            │   │
│  └─────────────────────┘  └─────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│                  Application Layer                      │
│  ┌─────────────────────┐  ┌─────────────────────────┐   │
│  │   CQRS Commands     │  │   DTOs & Validators     │   │
│  │   & Queries         │  │   Interfaces           │   │
│  └─────────────────────┘  └─────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│                     Domain Layer                        │
│  ┌─────────────────────┐  ┌─────────────────────────┐   │
│  │   Entities          │  │   Interfaces            │   │
│  │   Domain Logic      │  │   Domain Events         │   │
│  └─────────────────────┘  └─────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│                Infrastructure Layer                     │
│  ┌─────────────────────┐  ┌─────────────────────────┐   │
│  │   Entity Framework  │  │   Identity Provider     │   │
│  │   DbContext         │  │   External Services     │   │
│  └─────────────────────┘  └─────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

## ✨ Features

### Core Architecture
- **Clean Architecture** with clear layer separation
- **CQRS Pattern** with MediatR
- **Entity Framework Core** for data access
- **Dependency Injection** throughout all layers
- **API Versioning** support
- **Comprehensive Unit Testing** with xUnit and FluentAssertions

### Authentication & Authorization
- **JWT Bearer Authentication** with configurable expiration
- **Claims-based Security** for fine-grained permissions
- **Custom Identity Management** with Entity Framework Identity
- **Secure Password Policies**

### Data Management
- **Entity Framework Core 9** with MySQL support
- **Database Migrations** with automatic seeding
- **Soft Delete Implementation** with audit trail preservation
- **Comprehensive Auditing** with creation/modification tracking
- **Global Query Filters** for soft-deleted entities

### API Features
- **RESTful API Design** with OpenAPI/Swagger documentation
- **Request/Response Validation** with FluentValidation
- **Structured Error Handling** with custom middleware
- **CORS Configuration** for cross-origin requests
- **HTTP Logging** for debugging and monitoring

### Frontend Features
- **Angular 21** client application with latest features
- **PrimeNG 21** UI Components for rich user interface
- **JWT Authentication** integration with API
- **Responsive Design** with modern UI/UX
- **Standalone Components** support
- **Signals** and reactive programming features

### Logging & Monitoring
- **Serilog Integration** with multiple sinks (Console, File, Database)
- **Structured Logging** with contextual information
- **Health Checks** for application monitoring
- **Performance Monitoring** capabilities

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK
- MySQL Server 8.0+
- Visual Studio 2022 or Visual Studio Code
- Node.js 18+ (for Angular client)
- Angular CLI 21.0.3+

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/andyblem/CleanArchitectureTemplate.git
   cd CleanArchitectureTemplate
   ```

2. **Configure the database connection**
   
   Update `appsettings.json` and `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;user=root;password=YourPassword;port=3306;database=cleanarchitecturedb"
     }
   }
   ```

3. **Configure JWT settings**
   ```json
   {
     "JWTSettings": {
       "Key": "YourSecretKeyHere_MustBeAtLeast64CharactersLongForHMACSHA512Algorithm",
       "Issuer": "CoreIdentity",
       "Audience": "CoreIdentityUser",
       "DurationInMinutes": "60"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project CleanArchitecture.Infrastructure.Persistence --startup-project CleanArchitecture.Presentation.Web.Api
   ```

5. **Run the Web API**
   ```bash
   dotnet run --project CleanArchitecture.Presentation.Web.Api
   ```

6. **Run the Angular Client**
   ```bash
   cd CleanArchitecture.Presentation.Web.Client
   npm install
   ng serve
   ```

7. **Access the Applications**
   - **Web API**: `http://localhost:5500`
   - **Swagger UI**: `http://localhost:5500/swagger`
   - **Angular Client**: `http://localhost:4200`

## 🔐 Authentication

### Default Credentials
- **Username**: `admin@email.com`
- **Password**: `Password12345!`

### Getting a JWT Token
```bash
POST /api/accounts/authenticate
Content-Type: application/json

{
  "email": "admin@email.com",
  "password": "Password12345!"
}
```

### Using the JWT Token
Include the token in the Authorization header:
```bash
Authorization: Bearer your-jwt-token-here
```

## 📚 Project Structure

```
CleanArchitectureTemplate/
├── CleanArchitecture.Domain/                 # Domain Layer
│   ├── Common/                               # Shared domain concepts
│   ├── Entities/                             # Domain entities
│   └── Interfaces/                           # Domain interfaces
├── CleanArchitecture.Application/            # Application Layer
│   ├── DTOs/                                 # Data Transfer Objects
│   ├── Features/                             # CQRS Commands & Queries
│   │   ├── CQRS/                            # Command/Query handlers
│   │   ├── Parameters/                       # Request parameters
│   │   ├── Requests/                         # MediatR requests
│   │   └── Validators/                       # FluentValidation validators
│   ├── Interfaces/                           # Application interfaces
│   └── Wrappers/                             # Response wrappers
├── CleanArchitecture.Infrastructure.Persistence/ # Data Layer
│   ├── Contexts/                             # EF DbContext
│   ├── Extensions/                           # EF extensions
│   └── Seeders/                              # Database seeders
├── CleanArchitecture.Infrastructure.IdentityProvider/ # Identity Layer
│   ├── DTOs/                                 # Identity DTOs
│   └── Services/                             # Authentication services
├── CleanArchitecture.Infrastructure.Shared/  # Shared Infrastructure
│   ├── Identity/                             # Identity models
│   └── Services/                             # Shared services
├── CleanArchitecture.Presentation.Web.Api/   # API Layer
│   ├── Controllers/                          # API controllers
│   ├── Extensions/                           # API extensions
│   └── Services/                             # API-specific services
├── CleanArchitecture.Presentation.Web.Client/ # Angular Frontend
│   ├── src/                                  # Angular source code
│   │   ├── app/                             # Application components
│   │   ├── assets/                          # Static assets
│   │   └── environments/                    # Environment configs
│   ├── angular.json                         # Angular CLI config
│   ├── package.json                         # NPM dependencies
│   └── ...                                   # Angular configuration
└── Tests/                                    # Test Projects
    ├── CleanArchitecture.UnitTests/          # Unit tests
    ├── CleanArchitecture.IntegrationTests/   # Integration tests
    └── CleanArchitecture.FunctionalTests/    # Functional tests
```

## 🎯 API Endpoints

### Books API
```bash
GET    /api/v1/Books/GetList              # Get paginated books list
GET    /api/v1/Books/Get/{id}             # Get book by ID
POST   /api/v1/Books/Post                 # Create new book
PUT    /api/v1/Books/Put/{id}             # Update book
DELETE /api/v1/Books/Delete/{id}          # Soft delete book
```

### Accounts API
```bash
POST   /api/accounts/authenticate          # User authentication
GET    /api/accounts/claims               # Get user claims
GET    /api/accounts/userid               # Get current user ID
```

### Health Checks
```bash
GET    /health                            # Application health status
```

## 🅰️ Angular Frontend

The Angular client application is generated with [Angular CLI](https://github.com/angular/angular-cli) version 21.0.3 and uses Angular 21.

### Key Angular 21 Features
- **Standalone Components** by default
- **New Control Flow** syntax (`@if`, `@for`, `@switch`)
- **Signals** for reactive programming
- **Material 3** design system support
- **Improved SSR** capabilities
- **Enhanced Performance** optimizations

### Development Server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

### Code Scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

### Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

### Running Unit Tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

### Running End-to-End Tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

### Angular CLI Help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.

### Angular Features
- **PrimeNG 21 Components** for rich UI elements
- **Authentication Guards** for route protection
- **HTTP Interceptors** for automatic JWT token attachment
- **Error Handling** with user-friendly messages
- **Responsive Layout** with modern design
- **State Management** for application data
- **Modern Angular Architecture** with standalone components

## 🔧 Configuration

### Database Configuration
The application supports MySQL with Entity Framework Core. Configure your connection string in `appsettings.json`:

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;user=root;password=;port=3306;database=cleanarchitecturedb"
  }
}
```

### JWT Configuration
Configure JWT settings for authentication:

```json
{
  "JWTSettings": {
    "Key": "C1CF4B7DC4C4175B6618DE4F55CA4E9B8F2A7D6C5E4F3B2A1987654321ABCDEF",
    "Issuer": "CoreIdentity",
    "Audience": "CoreIdentityUser",
    "DurationInMinutes": "60"
  }
}
```

### Logging Configuration
Serilog is configured to log to console, file, and database:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "MySQL",
        "Args": {
          "connectionString": "server=localhost;user=root;password=;port=3306;database=cleanarchitecturedb",
          "tableName": "ApplicationLogs"
        }
      }
    ]
  }
}
```

## 🗃️ Database Features

### Entity Framework Core Architecture
The project uses Entity Framework Core with DbContext pattern for data access:

- **ApplicationDbContext** serves as the main data access layer
- **CQRS handlers** interact directly with DbContext
- **No Repository Pattern** - commands and queries use DbContext directly
- **Audit interceptors** handle automatic auditing

### Soft Delete Implementation
All entities implement `IAuditable` interface for comprehensive auditing:

```csharp
public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    string CreatedBy { get; set; }
    DateTime? ModifiedAt { get; set; }
    string? ModifiedBy { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string? DeletedBy { get; set; }
}
```

### Soft Delete Usage
```csharp
// In your command handlers
context.SoftRemove(entity);
await context.SaveChangesAsync();
```

### Audit Trail
- **Creation tracking**: `CreatedAt`, `CreatedBy`
- **Modification tracking**: `ModifiedAt`, `ModifiedBy`
- **Soft delete tracking**: `IsDeleted`, `DeletedAt`, `DeletedBy`
- **Global query filters** automatically exclude soft-deleted entities

## 🧪 Testing

The template includes comprehensive testing setup:

### Backend Testing
```bash
# Unit Tests
dotnet test CleanArchitecture.UnitTests

# Integration Tests
dotnet test CleanArchitecture.IntegrationTests

# Functional Tests
dotnet test CleanArchitecture.FunctionalTests
```

### Frontend Testing
```bash
# Angular Unit Tests
cd CleanArchitecture.Presentation.Web.Client
ng test

# Angular E2E Tests
ng e2e
```

### Test Coverage
Tests include:
- **Command/Query handlers**
- **Validation logic**
- **Entity Framework operations**
- **API endpoint functionality**
- **Authentication/Authorization**
- **Angular components and services**

## 🔒 Security Features

### Authentication
- **JWT Bearer tokens** with configurable expiration
- **Secure password hashing** using Identity framework
- **Token refresh** capabilities
- **Angular authentication guards**

### Authorization
- **Claims-based permissions**
- **API endpoint protection**
- **Route guards in Angular**

### Data Protection
- **SQL injection prevention** via parameterized queries
- **CORS configuration** for controlled access
- **Input validation** at multiple layers
- **Error handling** without sensitive data exposure
- **XSS protection** in Angular

## 🚀 Deployment

### Production Configuration
1. **Update connection strings** for production database
2. **Configure JWT settings** with production-grade keys
3. **Set up proper logging** configuration
4. **Configure CORS** for production domains
5. **Enable HTTPS** for secure communication
6. **Build Angular app** for production

### Angular Production Build
```bash
cd CleanArchitecture.Presentation.Web.Client
ng build --configuration production
```

### Docker Support
Docker configuration can be added for containerized deployment of both API and Angular app.

### Health Checks
Built-in health checks at `/health` endpoint for monitoring.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards
- Follow Clean Architecture principles
- Write comprehensive unit tests
- Use meaningful commit messages
- Document public APIs
- Follow C# and Angular coding conventions

## 🏗️ Architecture Decisions

### Why No Repository Pattern?
This template intentionally **does not use the Repository Pattern** because:

1. **Entity Framework is already a Repository** - DbContext provides the repository functionality
2. **Avoid over-abstraction** - Additional repository layer adds complexity without benefits
3. **Direct EF access** - CQRS handlers work directly with DbContext for better performance
4. **Testability** - EF Core's in-memory provider allows easy testing without mocking
5. **LINQ support** - Direct access to IQueryable for complex queries

### Data Access Pattern
```csharp
// Command Handler Example
public class CreateBookCommandHandler : IRequestHandler<CreateBookRequest, Response<int>>
{
    private readonly IApplicationDbContext _context;
    
    public async Task<Response<int>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var book = new Book { /* properties */ };
        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken);
        return Response<int>.Success(book.Id);
    }
}
```

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Clean Architecture** by Robert C. Martin
- **MediatR** for CQRS implementation
- **Entity Framework Core** for data access
- **Angular** and **Angular CLI** for frontend framework
- **PrimeNG** for UI components
- **Serilog** for structured logging
- **FluentValidation** for validation logic
- **xUnit** and **FluentAssertions** for testing
- **Sakai** web template

## 📞 Support

For support and questions:
- Create an issue in the GitHub repository
- Check the documentation in the wiki
- Review the test projects for usage examples

---

**Built with ❤️ using .NET 9, Angular 21, and Clean Architecture principles**
