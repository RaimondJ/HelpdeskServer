# HelpdeskServer

Helpdesk server made with ASP.NET Core 8.0

## Installation
- git clone
- cd to the folder
- Install dotnet-ef: **dotnet tool install --global dotnet-ef**
- Change sql connection string in appsettings.json
- Create a new database of your choice, here it is called 'helpdesk'
- Run **dotnet ef database update** to generate tables
- Run application 