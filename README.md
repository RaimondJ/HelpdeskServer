# HelpdeskServer

Helpdesk server made with ASP.NET Core 8.0

Database used here is MariaDB, but all other sql databases should work too, you just have to configure appsettings.json database connection string.

The database name here is helpdesk, and the single table name will be posts, that will store all posts, even the deleted ones.

## Installation
- git clone
- cd to the folder
- Install dotnet-ef: **dotnet tool install --global dotnet-ef**
- Change sql connection string in appsettings.json
- Create a new database of your choice, here it is called 'helpdesk'
- Run **dotnet ef database update** to generate tables
- Run application 
