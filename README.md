# Framework.Core
Framework.Core is a right way to build your Web application in ASP.NET Core (MVC 6).
Its cross-platform compatibility makes to deploy your application on Windows / Linux / MacOS.
It was thought to reduce developement costs and includes natively, for example, a generic API to use CRUD operations.

##Getting started with generic API
###Initialize 3 projects and add Framework.Core as reference

Sample projects :

1. Web application (ASP.NET Core MVC 6 => framework netcoreapp1.0)
2. Common (ASP.NET Core class library => framework netstandard1.6)

At that time, the service layer and data layer project is useless because the Framework.Core manage automatically the CRUD operations.

###Initialize your **appsettings.json**

In your **appsettings.json**, add this content :

```json
{
  "Framework": {
    "Configuration": {
      "Models": {
        "Assembly": "YourModelsAssembly",
        "NamespaceModels": "YourModelsNamespace",
        "Suffix": "YourModelsSuffix"
      }
    }
  }
}
```

###Initialize your own configuration file and add connection string

In your Web application project, add your own configuration file named **appsettings.YourMachineName.json** and add the Json content below to use connection string :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourSqlServerConnectionString"
  }
}
```

_If you build in release mode, make sure you are added the configuration file **appsettings.Production.json** and added the Json content to use connection string._

Based on this settings, Framework.Core initialize the application context like :

- Data models (Entity Framework Code First)
- Database context

### Configure your **startup.cs** to use Framework.Core
####- Use **appsettings.json** and **appsettings.YourMachineName.json**

Replace the constructor content by this code :

    Configuration = ConfigurationManager.Configure(env.ContentRootPath);

####- Initialize Framework.Core and add database context as service

In the **ConfigureServices** method and after **services.AddMvc();**, add the following code :

```cs
services.AddDbContext<DatabaseContext>(
  Configuration["ConnectionStrings:DefaultConnection"],
  GetType().Namespace);

services.Initialize();
```

The **AddDbContext** method define the **DatabaseContext** as scoped. It means that for each HTTP requests scope, the .NET Framework could be create a new instance of **DatabaseContext**.

The **Initialize** method initialize the context of Framework.Core.

####- Use code first migration on application launch

To use code first migration, add the code below at the end of the **Configure** method.

```cs
using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<DatabaseContext>().MigrateDatabase();
}
```

####- Create your first data model.

In your **common** project, add a new class named *UserModel* and add the code below :

```cs
[Table("Users")]
public class UserModel
{
  [Key]
  [Column("UserId")]
  public int UserId { get; set; }

  [Column("FirstName")]
  public string FirstName { get; set; }

  [Column("LastName")]
  public string LastName { get; set; }

  [Column("Account")]
  public string Account { get; set; }
}
```

####- Create your first migration

Open a new PowerShell window and type this command lines :
    cd YourWebApplicationPath
    dotnet ef migrations add Init

####- Execute the Web application and use generic API

Run the Web application in debug mode with Visual Studio 2015 or execute the command line **dotnet run** in PowerShell.

To use the generic API, type this URL :
    http://localhost:5000/api/user

If all is alright, you should receive this content :

```json
[]
```

##Next steps

For more details, please see the wiki page.

##Advantages

- Increase productivity for CRUD developments
- Propose best practices natively
- Designed flexibility for large or simple projects
- Fast ramp-up
- Ready to use with little configuration
- Model first (optional)

##Technical Stack
###Tools / Framework

- Visual Studio 2015 Update 3 (or later)
- ASP.NET Core (MVC 6 + Web API 2)
- Entity Framework Core
- Dapper.NET

###Concept

- Native Dependency Injection + Injection of Control (ASP.NET Core)
- Model - View - Controller (MVC)
- MVVM Pattern
- Multitier architecture
- Genericity
- Reflection

###Design Principles

- KISS
- Single Responsability

##Performance
###Environments
| Name       | Core | Memory (GB) | OS                  | Tool            |
| ---------- | ---- | ----------- | ------------------- | --------------- |
| WRK        |    8 |          28 | Ubuntu Server 16.10 | WRK             |
| Web        |    4 |          14 | Ubuntu Server 16.10 | Kestrel         |
| SQL Server |    8 |          28 | Ubuntu Server 16.10 | SQL Server 2016 |

These environments have been deployed on Windows Azure.

###Context
This performance test was executed with WRK benchmark tool.
SQL Server environment is limited to 30 concurrent connections. So, I have used :

- Threads : 16
- Connections open : 30
- Duration : 10 minutes

###WRK Results
| Thread stars | Average   | Standard Deviation | Max     | +/- Standard Deviation |
| ------------ | --------- | ------------------ | ------- | ---------------------- |
| Latency      |   31.30ms |           113.29ms |   1.68s | 94.57%                 |
| Req/Sec      |  152.90   |            39.16   | 240.00  | 85.89%                 |

Requests / sec : 2099.01 ; Transfer / sec : 188.58KB
