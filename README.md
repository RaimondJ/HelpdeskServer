# HelpdeskServer

Helpdesk server made with ASP.NET Core 8.0

Database used here is MariaDB, but all other sql databases should work too, you just have to configure appsettings.json database connection string.

The database name here is helpdesk, and the single table name will be posts, that will store all posts, even the deleted ones.

## Api endpoints and request methods

- ``/api/helpdesk/post`` (Method: POST) - Will create a new post and store it in the database, request parameters should contain what's in class ``DTO.Request.PostDto``
- ``/api/helpdesk/posts`` (Method: GET) - Returns all posts that are currently open. The endpoint returns subject, description, start date, deadline date, boolean if post is expiring or not (Less than 1h to the deadline date or expired), and a string of how many days, hours, minutes are left until expiring. If post is expired, the string will be about that the post is expired.
- ``/api/helpdesk/posts_count`` (Method: GET) - Returns an integer of all posts count ever created. That is used in frontend's main page.
- ``/api/helpdesk/post/{id}`` (Method: DELETE) - Deletes a post by the given id. If post doesn't exist or there was another failure, the boolean of success in ``DTO.Response.DeletePostDto`` will be false, otherwise, if deleted successfully, true.

## Tests
Tests are included, for tests the app is using a local temporary database. So if you want to write custom sql queries and test them, they will fail. I tried to find a fix for that, but in the end, couldn't fix it. I guess the solution is to create a temporary database in your actual sql server and use that.

## Installation
- git clone
- cd to the folder
- Install dotnet-ef: **dotnet tool install --global dotnet-ef**
- Change sql connection string in appsettings.json
- Create a new database of your choice, here it is called 'helpdesk'
- Run **dotnet ef database update** to generate tables
- Run application 
