# EventEase
# EventEase 🎉

EventEase is an ASP.NET Core MVC web application for managing venues, events, and bookings in one place.

---

## 🚀 Features

- Manage Venues (Create, Edit, Delete)
- Manage Events linked to Venues
- Manage Bookings linked to Events
- Prevent double bookings for the same event/date
- Clean and responsive UI using Bootstrap

---

## 🛠 Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap (UI Styling)

---

## 📂 Project Structure

- Controllers → Handles application logic
- Models → Database entities (Venue, Event, Booking)
- Views → UI pages (Razor)
- Migrations → Database schema
- wwwroot → Static files (CSS, JS)

---

## ▶️ How to Run the Project

### 1. Clone the repository
bash
git clone https://github.com/TashreeqPhipps/EventEase3.git
2. Open the project
Open the solution file (EventEase.sln) in Visual Studio
3. Restore dependencies
dotnet restore
4. Build the project
dotnet build
5. Update the database
dotnet ef database update
6. Run the application
dotnet run

👨‍💻 Author
Tashreeq Phipps ST10031953
