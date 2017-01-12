# Framework.Core
Framework.Core is a right way to build your Web application in ASP.NET Core (MVC 6).
Its cross-platform compatibility makes to deploy your application on Windows / Linux / MacOS.
It was thought to reduce developement costs and includes natively, for example, a generic API to use CRUD operations.

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
