# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ASI.TCL.CMFT is a Communication Multi-Function Terminal (CMFT) system for Taipei Metro's Circular Line. It's a modular WPF application built on .NET 8 using Domain-Driven Design (DDD) principles and the Prism framework for module management.

## Common Development Commands

### Building the Solution
```powershell
dotnet build ASI.TCL.CMFT.sln
```

### Running the Main Application
Set `ASI.TCL.CMFT.WPF.CMFTApp` as the startup project and run from Visual Studio, or:
```powershell
dotnet run --project WPF\ASI.TCL.CMFT.WPF.CMFTApp\ASI.TCL.CMFT.WPF.CMFTApp.csproj
```

### Database Migrations

Create new migration:
```powershell
dotnet ef migrations add <MigrationName> `
    --project .\Infrastructure\ASI.TCL.CMFT.Infrastructure.EFCore\ASI.TCL.CMFT.Infrastructure.EFCore.csproj `
    --startup-project .\Infrastructure\ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool\ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool.csproj
```

Update database:
```powershell
dotnet ef database update `
  --project ./Infrastructure/ASI.TCL.CMFT.Infrastructure.EFCore/ASI.TCL.CMFT.Infrastructure.EFCore.csproj `
  --startup-project ./Infrastructure/ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool/ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool.csproj
```

Remove migration:
```powershell
dotnet ef migrations remove `
  --project .\Infrastructure\ASI.TCL.CMFT.Infrastructure.EFCore\ASI.TCL.CMFT.Infrastructure.EFCore.csproj `
  --startup-project .\Infrastructure\ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool\ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool.csproj `
  --force
```

## Architecture Overview

### Clean Architecture Layers

The project follows Clean Architecture with these main layers:

1. **Domain Layer** (`Domain/`)
   - Core business entities and domain logic
   - Uses event sourcing pattern with `AggregateRoot<TId>`
   - Domain modules: Auth, DMD (Dynamic Message Display), PA (Public Address), SYS (System)

2. **Application Layer** (`Application/`)
   - Application services and use cases
   - CQRS pattern with Commands and Queries
   - Uses Dapper for read operations

3. **Infrastructure Layer** (`Infrastructure/`)
   - Entity Framework Core for data persistence
   - PostgreSQL database provider
   - Implements repositories and unit of work patterns

4. **Presentation Layer** (`WPF/`)
   - Modular WPF application using Prism
   - MVVM pattern with ViewModels
   - Material Design theming

### Architecture Patterns (10 key patterns)

1. **Clean Architecture**: Four-layer separation (Domain, Application, Infrastructure, Presentation)
2. **Domain-Driven Design**: Bounded contexts for different system modules
3. **Event Sourcing**: Aggregates track state changes through events (`AggregateRoot<TId>`)
4. **CQRS**: Separate command and query models with dedicated handlers
5. **Hexagonal Architecture**: Interface-based isolation of external dependencies
6. **Modular Monolith**: Prism framework for modular WPF application structure
7. **Layered Architecture**: Clear separation of responsibilities across layers
8. **Plugin Architecture**: Dynamic module loading via Prism
9. **Repository Pattern**: Abstracted data access layer (`IRepository<T, TId>`)
10. **Unit of Work Pattern**: Unified transaction management (`IUnitOfWork`)

### Design Patterns (15 key patterns)

#### Creational Patterns
- **Factory Pattern**: DbContextFactory for database context creation
- **Dependency Injection**: Unity container for IoC

#### Structural Patterns  
- **Adapter Pattern**: Infrastructure layer adapting different data sources
- **Facade Pattern**: ApplicationService providing unified interfaces

#### Behavioral Patterns
- **Command Pattern**: CQRS command classes with handlers
- **Observer Pattern**: Domain Events mechanism
- **Strategy Pattern**: Different query strategy implementations
- **Template Method Pattern**: AggregateRoot's When/EnsureValidState abstract methods

#### Domain-Specific Patterns
- **Aggregate Pattern**: AggregateRoot managing entity aggregates
- **Value Object Pattern**: Value<T> abstract class implementation
- **Entity Pattern**: Entity<TId> base entity class
- **Domain Event Pattern**: Domain event recording and handling
- **Specification Pattern**: Query condition encapsulation

#### UI Patterns
- **MVVM Pattern**: Model-View-ViewModel for WPF
- **Mediator Pattern**: Prism EventAggregator for event mediation

### Module Structure

Each functional module (DMD, PA, SYS, etc.) follows this structure:
- Domain: Entities, events, value objects
- Application: Services, commands, queries, DTOs
- WPF Module: Views, ViewModels, module registration

### System Modules

- **DMD**: Dynamic Message Display management
- **PA**: Public Address system control
- **SYS**: System administration and user management
- **Auth**: Authentication and authorization
- **CCTV**: Video surveillance integration
- **DLTS**: Direct Line Telephone System
- **OTCS**: On-Train Communication System
- **Tetra**: TETRA radio communication
- **Alarm**: System alarm management

## Key Technologies

- .NET 8 (target framework)
- WPF with Prism 8.1.97
- Entity Framework Core 8.0.0 with PostgreSQL
- Material Design Themes 5.0.0
- Dapper for queries
- Unity for dependency injection

## Technical Architecture Features

- **Event-Driven Architecture**: Loose coupling through Domain Events
- **Authorization Control**: Fine-grained permissions via Authority Attributes
- **Audit Functionality**: Automatic creation/modification tracking via AuditableEntity
- **Multi-Database Support**: EF Core configuration supporting PostgreSQL
- **Modular UI**: Prism Region Manager for view area management
- **Asynchronous Processing**: Comprehensive use of async/await patterns
- **Type Safety**: Strong typing with Value Objects and Entity IDs
- **State Validation**: Built-in aggregate state validation (EnsureValidState)
- **Change Tracking**: Event sourcing for complete audit trails

## Development Notes

- The solution includes multiple startup projects for different purposes
- Main application: `ASI.TCL.CMFT.WPF.CMFTApp`
- Migration tool: `ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool`
- Test application: `ASI.TCL.CMFT.WPF.Test`
- Specific module apps: `ASI.TCL.CMFT.WPF.DMDSetting`

When adding new features:
1. Start with domain modeling in the appropriate Domain project
2. Implement application services in the corresponding Application project
3. Create UI components in the relevant WPF Module project
4. Register new views and services in the appropriate module class and bootstrapper

The codebase uses Chinese comments in some areas, particularly for UI-related functionality and business logic descriptions.