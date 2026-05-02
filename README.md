# CoreFitness – Gym Portal (ASP.NET Core MVC)

## 📌 About the project
CoreFitness is a full-stack web application built with ASP.NET Core MVC.  
The system allows users to create accounts, manage memberships, book classes, and track their activity.

The project is developed as part of an ASP.NET course with focus on:
- Clean Architecture
- Domain-Driven Design (DDD)
- Authentication & Authorization (ASP.NET Identity)
- Entity Framework Core (Code First)
- Responsive UI based on Figma design

---

## 🚀 Features

### 👤 Authentication
- User registration & login
- External login (Google)
- Role-based authorization (Admin / Member)

### 💳 Membership
- Create and manage memberships
- View current membership plan

### 🏋️ Gym Classes
- View available classes
- Book classes
- Cancel bookings

### 📊 User Dashboard
- View bookings
- View membership details

### 🛠 Admin
- Create gym classes
- Delete gym classes


---


## 🧠 Design Patterns & Concepts

- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- Result Pattern (`ServiceResult`)
- Dependency Injection
- Separation of Concerns

---

## 🗄 Database

- Entity Framework Core (Code First)
- SQL Server / LocalDB
- Migrations supported

---

## 🧪 Testing

- Unit tests using xUnit + Moq
- Integration tests using InMemory Database
- Tested business rules:
  - Prevent double booking
  - Prevent booking full classes
  - Validate class existence

---

## 🎨 Frontend

- Built from Figma design
- Responsive (Mobile-first approach)
- Flexbox & CSS Grid
- Reusable Partial Views

---

## ⚙️ Getting Started

### 1. Clone repository

Run this in your terminal:

`git clone https://github.com/aleksandarcurcin-del/CoreFitness.git`

Then enter the project folder:

`cd CoreFitness`

---

### 2. Open solution file

Open the solution in Visual Studio:

`CoreFitness2.sln`

---

### 3. Setup database

Run migrations using Package Manager Console:

`Update-Database`

---

### 4. Run the application

Start the application in Visual Studio:

`F5 / Start Debugging`
