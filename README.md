Shipping-Management-System Project
Overview
The Shipping-Management-System project is a .NET 9-based web application designed to manage shipping operations. It includes features for managing shipping types, city costs, courier regions, weight settings, and more. The project is structured into multiple layers to ensure scalability, maintainability, and separation of concerns.
---
Project Structure
The solution is divided into the following projects:
1.	ITI.Shipping.APIs:
•	This is the main API project that exposes endpoints for managing shipping operations.
•	Built using ASP.NET Core with support for JWT Authentication and Swagger for API documentation.
2.	ITI.Shipping.Core.Application:
•	Contains the application logic and service interfaces.
•	Acts as the bridge between the API and the domain layer.
3.	ITI.Shipping.Core.Domin:
•	Represents the core domain models and business rules.
•	Includes entities, enums, and shared logic.
4.	ITI.Shipping.Infrastructure.Presistence:
•	Handles data persistence using Entity Framework Core with SQL Server.
•	Includes migrations and database context configuration.
---
Tools and Technologies
The project leverages the following tools and libraries:
Frameworks and Languages:
•	.NET 9: The latest version of the .NET platform.
•	C# 13.0: The programming language used for development.
API and Authentication:
•	Microsoft.AspNetCore.Authentication.JwtBearer: For secure JWT-based authentication.
•	Microsoft.AspNetCore.OpenApi: For OpenAPI/Swagger integration.
Database and ORM:
•	Entity Framework Core:
•	Microsoft.EntityFrameworkCore.SqlServer: For SQL Server database integration.
•	Microsoft.EntityFrameworkCore.Tools: For managing migrations.
•	Microsoft.EntityFrameworkCore.Proxies: For lazy loading support.
API Documentation:
•	Swashbuckle.AspNetCore.SwaggerGen: For generating Swagger/OpenAPI documentation.
•	Swashbuckle.AspNetCore.SwaggerUI: For providing a user-friendly Swagger UI.
Identity and Security:
•	Microsoft.AspNetCore.Identity.EntityFrameworkCore: For managing user authentication and roles.
•	Microsoft.Extensions.Identity.Stores: For identity store extensions.
---
Features
1.	Shipping Type Management:
•	Add, update, delete, and retrieve shipping types.
2.	Special City Costs:
•	Manage custom shipping costs for specific cities.
3.	Special Courier Regions:
•	Assign couriers to specific regions.
4.	Weight Settings:
•	Configure weight-based shipping costs.
5.	Authentication:
•	Secure endpoints using JWT-based authentication.
6.	API Documentation:
•	Explore and test APIs using Swagger UI.
---
Folder Structure
ITI.Shipping/
├── ITI.Shipping.APIs/
│   ├── Controllers/
│   ├── Filters/
│   ├── appsettings.json
│   └── ITI.Shipping.APIs.csproj
├── ITI.Shipping.Core.Application/
│   ├── Abstraction/
│   └── ITI.Shipping.Core.Application.csproj
├── ITI.Shipping.Core.Domin/
│   ├── Entities/
│   ├── Enums/
│   └── ITI.Shipping.Core.Domin.csproj
├── ITI.Shipping.Infrastructure.Presistence/
│   ├── Data/
│   ├── Migrations/
│   └── ITI.Shipping.Infrastructure.Presistence.csproj
---
Future Enhancements
1.	Add unit tests for all controllers and services.
2.	Implement caching for frequently accessed data.
3.	Add support for multi-tenancy.
---
Contributing
Contributions are welcome! Please follow these steps:
1.	Fork the repository.
2.	Create a new branch for your feature or bug fix.
3.	Submit a pull request with a detailed description of your changes.
--- 
