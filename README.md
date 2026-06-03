# EventEase 🎉

EventEase is an ASP.NET Core MVC web application created for the CLDV6211 POE. The application allows users to manage venues, events, and bookings in one system. Part 2 enhances the application by adding local cloud storage using Azurite, image uploads for venues, search functionality, validation, and delete restrictions.

---

## Features

- Create, view, edit, and delete venues
- Upload venue images using local blob storage through Azurite
- Create, view, edit, and delete events linked to venues
- Create, view, edit, and delete bookings linked to events
- Prevent double bookings for the same event on the same date
- Restrict deleting venues that are linked to active bookings
- Restrict deleting events that are linked to active bookings
- Search venues, events, and bookings
- Clean responsive user interface using Bootstrap

---

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server LocalDB
- Azure Storage Blobs
- Azurite Local Blob Storage Emulator
- Bootstrap
- Visual Studio 2022

---

## Project Structure

- `Controllers` - Handles application logic
- `Models` - Contains the database entities
- `Views` - Contains Razor UI pages
- `Services` - Contains the blob storage service
- `Migrations` - Contains database migration files
- `wwwroot` - Contains CSS, JavaScript, and static files

---

## How to Run the Application

After opening the project in Visual Studio, make sure Azurite is running before testing image uploads.

### Step 1: Open the Project

Open the solution file:

EventEase.sln

in Visual Studio 2022.

### Step 2: Start Azurite

Open the Visual Studio terminal or Developer PowerShell and run:

azurite.cmd --skipApiVersionCheck

Keep this terminal open while running the application.

### Step 3: Restore Packages

In the Package Manager Console or terminal, run:

dotnet restore

### Step 4: Build the Project

Run:

dotnet build

The project should build successfully with 0 errors.

### Step 5: Update the Database

Run:

dotnet ef database update

This will create or update the local database using Entity Framework Core migrations.

### Step 6: Run the Application

Run the project in Visual Studio by clicking the green play button, or use:

dotnet run

The application will open in the browser.

---

## Important Notes

Azurite must be running when uploading venue images. If Azurite is not running, image upload functionality will not work.

The application uses LocalDB for database storage and Azurite for local blob image storage.

---

## Author

Tashreeq Phipps  
ST10031953
