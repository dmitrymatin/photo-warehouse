# photo-warehouse

# Basic requirements to start a project
* [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
* To open and run a project - Visual Studio 2019 and higher (or any text editor, but then all work with the project is done through [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/))
* MySQL8.0

# Project structure
* PhotoWarehouse.Domain - contains classes that describe the domain. ORM *(Entity Framework Core 5)* is used to create the corresponding tables in the database and represent them as entities in the app
* PhotoWarehouse.Data - contains classes that enable the ORM to connect domain entities with the database, as well as repository classes for executing queries to the database
* PhotoWarehouseApp - web application project

# Configuration
1. In PhotoWarehouseApp, in the appsettings.json file, check the connection string to the local database (you probably need to change the user name)
2. Add secret data to the application configuration:
  * When working in VS2019, right-click on the PhotoWarehouseApp project and select *"Manage User Secrets"*
      This will open a json file. You need to add the following content to it:
      ```json
      {
         "DbPassword": "<password_to_database_server>",
         "AdministratorEmail": "admin@pw.com",
         "AdministratorPassword": "<password_for_site_administrator>"
      }
      ```
      It is not necessary to change the administrator email (AdministratorEmail), it is fictitious

   * If you are not using VS2019, then each of the 3 settings can be set with the command `dotnet user-secrets set "<key>" "<value>"` </br>
For example, `dotnet user-secrets set "DbPassword" "pass123"`

3. Create a database by applying migrations from the PhotoWarehouse.Data project
First, you need to make sure that the tool for working with Entity Framework is installed: dotnet-ef.
To install it, you need to run the command `dotnet tool install --global dotnet-ef` <br/>
After installing dotnet-ef, go to the console in the folder with the PhotoWarehouse.Data project (In VS2019, in the context menu for the PhotoWarehouse.Data project, you can select the option to open in the terminal - *Open in Terminal*). </br>
Run the command `dotnet ef database update -s ..\PhotoWarehouseApp\PhotoWarehouseApp.csproj` </br>
The database should be created and can be seen in the databases list, for example, in MySQL Workbench. If the database cannot be created, there may be an issue with access rights and the database must be created manually.

5. Launching the app
* In VS2019 you can choose to run on either IIS Express or Kestrel (PhotoWarehouseApp in the dropdown next to the green launch button)
* From the console you can run the command `dotnet run`
* Once launched, you can log in as an administrator using the administrator name and password set in the AdministratorPassword configuration, and also create a new client user account


This stock photo project has been implemented as part of coursework at university (VLSU).
