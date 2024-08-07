# OliveBlazor

## Overview

**OliveBlazor** is a project template that provides a foundational setup for building a Blazor Server application with a Clean Architecture. It includes built-in authentication and authorization support using ASP.NET Core Identity, making it an excellent starting point for secure web applications.

### Key Features

- **Blazor Server**: A single-page application framework for building interactive web UIs using C# and .NET.
- **Clean Architecture**: An architectural pattern that promotes separation of concerns, testability, and maintainability.
- **Authentication & Authorization**: Integrated ASP.NET Core Identity for user authentication and role-based authorization.
- **Modular Structure**: Organized into UI, Core, and Infrastructure projects for a scalable and maintainable solution.
- **Customizable**: Easily extendable to fit your specific business needs and requirements.
- **Default Admin User**: A default admin user is seeded with the setup data for easy access and testing.

## Project Structure

The solution is structured into three main projects:

1. **UI Project** (`OliveBlazor.UI`)
   - The Blazor Server project responsible for rendering the user interface.
   - Contains Razor components, pages, and UI-specific services.

2. **Core Project** (`OliveBlazor.Core`)
   - Contains the domain and application logic, including business rules, entities, and service interfaces.
   - Organized into `Domain` and `Application` layers.

3. **Infrastructure Project** (`OliveBlazor.Infrastructure`)
   - Handles data access, external service integrations, and other infrastructure concerns.
   - Contains implementations for repositories, data context, and service integrations.

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or Visual Studio Code


### Email Confirmation Setup

Enable email confirmation from the EmailConfirmation section in the appsettings.json file  

To enable email confirmation functionality in this template, you need to configure an SMTP service.
This template uses [SMTP2GO](https://www.smtp2go.com/) for sending emails. Follow these steps to set it up:

#### Step 1: Create an SMTP2GO Account

1. Go to [SMTP2GO](https://www.smtp2go.com/) and sign up for an account.
2. After signing up, navigate to the **SMTP & API** section to find your SMTP credentials.

#### Step 2: Configure Email Settings in `appsettings.json`

Open the `appsettings.json` file in the `OliveBlazor.UI` project and update the `EmailSettings` section with your SMTP2GO credentials:

```json
"EmailSettings": {
  "SmtpServer": "mail.smtp2go.com",
  "SmtpPort": "2525",
  "UserName": "your_smtp2go_username",
  "Password": "your_smtp2go_password"
},
"EmailConfirmation": {
  "Enabled": true
},
```


### Generating the Database

To set up the database for this template, use Entity Framework Core tools to create the database named `OliveBlazor-DB`.

#### Step 1: Open Package Manager Console

1. Open Visual Studio.
2. Go to **Tools > NuGet Package Manager > Package Manager Console**.

#### Step 2: Run the EF Core Migration Command

In the Package Manager Console, navigate to the project containing your `DbContext` ( `OliveBlazor.Infrastructure`) and run the following command:

```shell
Update-Database 
```

### Default Admin User
Admin / Admin@123



