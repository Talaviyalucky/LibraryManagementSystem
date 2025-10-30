# 📚 Library Management System | ASP.NET Core MVC

Online **Library Management System** built using **ASP.NET Core MVC** and **Entity Framework Core**, created as a university-level project.  
The system enables administrators to manage books and users efficiently while allowing users to browse, borrow, and return books.

---

## 🧩 Features

### 📘 Book Catalog

- search books by title, author, or category
- Display available , total and issued books
- Display Total user

---

### 👤 User Accounts

- User registration and login
- Borrow and return books
- View current and past borrowed books

---

### 🧑‍💼 Admin Account

- Full control over the system
- Dashboard displaying:
  - Total books
  - Available books
  - Issued books
  - Total users
- Manage Books
  - Add new books
  - Edit book details
  - Delete books

---

## 🛠️ Technology Stack

- **Frontend:** HTML5, CSS3, Bootstrap 5
- **Backend:** ASP.NET Core MVC (C#)
- **Database:** Entity Framework Core with SQL Server / SQLite
- **Authentication:** Session-based login system

---

## ⚙️ Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/LibraryManagementSystem.git
   ```

2. **Open the project** in Visual Studio or VS Code.

3. **Configure the database** in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;"
   }
   ```

4. **Apply migrations (optional)**

   ```bash
   dotnet ef database update
   ```

5. **Run the project**
   ```bash
   dotnet run
   ```
   Visit the app at:  
   👉 `http://localhost:5000`

---

## 🧭 Project Structure

```
LibraryManagement/
├── Controllers/
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── BooksController.cs
│   └── HomeController.cs
│
├── Models/
│   ├── AppDbContext.cs
│   ├── User.cs
│   ├── Book.cs
│   └── Borrow.cs
│
├── Views/
│   ├── Admin/
│   ├── Auth/
│   ├── Books/
│   ├── Home/
│   └── Shared/
│
└── wwwroot/
```

---

## 👨‍💻 Created By

**Lucky Talaviya**  
📧Email:-23amtics435@gmail.com  
💻 GitHub Profile:- https://github.com/Talaviyalucky 

---

## Admin login

username-admin
password-admin123

## user login

username-DemoUser
password-123# LibraryManagement
