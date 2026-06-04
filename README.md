# EventEase 🎉

EventEase is an ASP.NET Core MVC web application created for the CLDV6211 Cloud Development POE. The application allows users to manage venues, events, event types, bookings, and image uploads in one system.

For Part 3, the application was upgraded from a local development solution to a live cloud-hosted application using Microsoft Azure. The system now uses Azure App Service for hosting, Azure SQL Database for cloud database storage, and Azure Blob Storage for storing uploaded venue and event images.

---

## Live Azure Website

Live application URL:

https://eventease-st10031953-f6gjasava7a3d9g9.southafricanorth-01.azurewebsites.net

---

## Part 3 Overview

Part 3 focuses on migrating the EventEase application from a local development environment to a live cloud environment. In Part 2, the application used LocalDB and Azurite for local testing. In Part 3, the application was deployed to Azure and connected to live cloud services.

The main Part 3 improvements include:

* Deployment to Azure App Service
* Migration from LocalDB to Azure SQL Database
* Migration from Azurite to Azure Blob Storage
* EventType model and table
* Event categorisation using predefined event types
* Advanced event filtering
* Venue image upload to Azure Blob Storage
* Event image upload to Azure Blob Storage
* Live testing of bookings and delete restrictions
* Reflective technical report and advanced cloud theory discussion

---

## Features

### Venue Management

* Create, view, edit, and delete venues
* Search venues by name or location
* Upload venue images
* Store uploaded venue images in Azure Blob Storage
* Display venue images in the Venues list and Details page
* Prevent deletion of venues that are linked to active bookings

### Event Management

* Create, view, edit, and delete events
* Link events to venues
* Link events to event types
* Upload event images
* Store uploaded event images in Azure Blob Storage
* Display event images in the Events list and Details page
* Show whether an event is Available or Booked
* Prevent deletion of events that are linked to active bookings

### Event Type Management

* Added an EventType model and table
* Events can be categorised by event type
* Predefined event types include:

  * Conference
  * Wedding
  * Workshop
  * Birthday
  * Meeting

### Advanced Event Filtering

The Events page includes advanced filtering options:

* Search by event name, venue, or event type
* Filter by event type
* Filter by start date
* Filter by end date
* Filter by availability

### Booking Management

* Create, view, edit, and delete bookings
* Link bookings to events
* Display booking information
* Prevent double bookings where required
* Change event status to Booked when a booking exists

---

## Azure Services Used

### Azure App Service

Azure App Service was used to host the ASP.NET Core MVC application online. This allowed the local Visual Studio project to be published to a live Azure website. App Service is suitable for this application because it provides managed hosting for web applications without needing to manually manage the underlying server infrastructure (Microsoft, 2025a).

### Azure SQL Database

Azure SQL Database was used as the live cloud database for EventEase. The local Entity Framework Core migrations were applied to the Azure SQL Database so that the live application could use the same database structure. Azure SQL Database was selected because EventEase uses relational data, including Venues, Events, EventTypes, and Bookings (Microsoft, 2026a).

### Azure Blob Storage

Azure Blob Storage was used to store uploaded venue and event images. Instead of saving image files directly inside the SQL database, the application stores the images in Blob Storage and saves the image URL in the database. This is appropriate because Blob Storage is designed for unstructured data such as images, documents, and binary files (Microsoft, 2023).

### Azure Resource Group

All cloud resources were organised inside the EventEaseRG resource group. This made it easier to manage the related Azure services in one place.

---

## Technologies Used

* ASP.NET Core MVC (.NET 8)
* C#
* Entity Framework Core
* SQL Server LocalDB
* Azure SQL Database
* Azure App Service
* Azure Blob Storage
* Azurite Local Blob Storage Emulator
* Bootstrap
* Visual Studio 2022
* GitHub

---

## Project Structure

* `Controllers` - Handles application logic
* `Models` - Contains database entities such as Venue, Event, EventType, and Booking
* `Views` - Contains Razor pages for the user interface
* `Services` - Contains the BlobStorageService for image upload and deletion
* `Migrations` - Contains Entity Framework Core migration files
* `wwwroot` - Contains CSS, JavaScript, and static files

---

## Database Migrations

The application uses Entity Framework Core migrations to manage database changes.

Important migrations include:

* Initial database creation
* AddEventType
* AddEventImageUrl

The `AddEventImageUrl` migration added the `ImageUrl` column to the Events table so that event images could be uploaded and displayed.

---

## Local Development Setup

### Step 1: Open the Project

Open the solution file in Visual Studio 2022:

```text
EventEase.sln
```

### Step 2: Start Azurite

For local image upload testing, Azurite must be running.

Open the Visual Studio terminal or Developer PowerShell and run:

```powershell
azurite.cmd --skipApiVersionCheck
```

Keep this terminal open while running the application locally.

### Step 3: Restore Packages

Run:

```powershell
dotnet restore
```

### Step 4: Build the Project

Run:

```powershell
dotnet build
```

The project should build successfully with 0 errors.

### Step 5: Update the Local Database

Run:

```powershell
Update-Database
```

or:

```powershell
dotnet ef database update
```

This creates or updates the local database using Entity Framework Core migrations.

### Step 6: Run the Application

Run the project in Visual Studio by clicking the green play button, or use:

```powershell
dotnet run
```

The application will open in the browser.

---

## Azure Deployment Setup

The application was deployed using the following Azure resources:

* Resource Group: EventEaseRG
* App Service: eventease-st10031953
* Azure SQL Database: EventEaseDb
* SQL Server: eventease-server-st10031953
* Storage Account: eventeasestorage31953
* Blob Container: venue-images

The live App Service uses environment variables for production configuration. Sensitive connection strings are not hard-coded into the application.

Important App Service environment variables include:

```text
ConnectionStrings__DefaultConnection
AzureStorage__ConnectionString
AzureStorage__ContainerName
```

---

## Cloud Migration Reflection

The project was first developed locally using LocalDB and Azurite. LocalDB was useful for testing relational database functionality, while Azurite allowed blob storage image upload functionality to be tested without using live Azure resources.

For Part 3, the application was migrated to Azure. This required creating a live Azure SQL Database and applying the Entity Framework Core migrations to it. It also required creating a live Azure Storage Account and blob container for uploaded images. The application was then published to Azure App Service through Visual Studio.

One important lesson from the migration was that local and cloud environments must be configured separately. Local development can use LocalDB and Azurite, while the live application uses Azure SQL Database and Azure Blob Storage. This separation is important in professional development because it prevents production secrets from being hard-coded and allows the same codebase to work in different environments.

Another lesson was that cloud services may behave differently from local services. For example, Azure SQL Database can sometimes take time to become available, especially when using a free or serverless configuration. Retrying the database migration after a short delay solved the issue.

Overall, the migration helped demonstrate how a local ASP.NET Core MVC application can be converted into a live cloud-hosted system using Azure services.

---

## Advanced Cloud Theory

### Cosmos DB Compared to Traditional Databases

Azure Cosmos DB differs from traditional relational databases because it is designed as a globally distributed NoSQL database service. Traditional databases such as SQL Server and Azure SQL Database store data in structured tables with rows, columns, primary keys, and foreign keys. This is useful for applications like EventEase, where data relationships are important, such as venues having events and events having bookings.

Cosmos DB is more flexible because it can store semi-structured data, such as JSON documents. It is also designed for horizontal scaling, global distribution, and low-latency access. This makes it useful for applications that need to handle large volumes of distributed data across different regions (Microsoft, 2025b).

For EventEase, Azure SQL Database was the better choice because the system uses structured relational data. However, Cosmos DB would be useful in a future version if the application needed globally distributed event analytics, activity logs, or flexible user data storage.

### Logic Apps and Sensitive Data

Azure Logic Apps allow developers to build automated workflows that connect systems and services. When Logic Apps handle sensitive data, security must be considered carefully. Sensitive values such as passwords, API keys, and connection strings should not be stored directly in workflow definitions. They should be protected using secure parameters or services such as Azure Key Vault.

Access control is also important. Only authorised users should be able to view, edit, or run workflows that handle sensitive data. Role-Based Access Control should be used to limit permissions. Workflow run history should also be managed carefully because inputs and outputs can expose sensitive information if they are not protected (Microsoft, 2025c).

If EventEase used Logic Apps in the future, they could be used to send booking confirmations or admin notifications. However, personal booking details and email addresses would need to be protected.

### Event Grid and Robust Workflows

Azure Event Grid supports event-driven workflows by allowing services to react when something happens. For example, when a file is uploaded to Azure Blob Storage, an event can trigger another service such as an Azure Function or Logic App. This creates a loosely coupled workflow where each service has a clear responsibility.

Event Grid can be combined with Azure Blob Storage, Azure Functions, Logic Apps, and App Service to create automated cloud workflows. For example, a future version of EventEase could trigger a workflow when a booking is created. This workflow could send a confirmation email, notify an administrator, or update an external calendar. Event Grid is useful because it supports scalable, event-driven application design (Microsoft, 2026b).

---

## Important Notes

For local development, Azurite must be running when testing image uploads.

For the live Azure version, images are stored in Azure Blob Storage and data is stored in Azure SQL Database.

The live application uses App Service environment variables for connection strings and storage settings.

Connection strings and access keys should not be committed to GitHub.

---

## Author

Tashreeq Phipps
ST10031953

---

## Reference List

Microsoft. 2023. *Introduction to Azure Blob Storage*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction (Accessed: 4 June 2026).

Microsoft. 2025a. *Overview of Azure App Service*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/app-service/overview (Accessed: 4 June 2026).

Microsoft. 2025b. *Azure Cosmos DB overview*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/cosmos-db/overview (Accessed: 4 June 2026).

Microsoft. 2025c. *Azure serverless - Azure Logic Apps*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-serverless-overview (Accessed: 4 June 2026).

Microsoft. 2026a. *What is the Azure SQL Database service?* Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/azure-sql/database/sql-database-paas-overview (Accessed: 4 June 2026).

Microsoft. 2026b. *Introduction to Azure Event Grid*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/azure/event-grid/overview (Accessed: 4 June 2026).
