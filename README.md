# 🎓 Student Course Registration System

A complete **ASP.NET Core MVC** application for managing student course registrations, built using **Entity Framework Core**, **Identity**, and **Email integration** via Gmail.

---

## 🧩 Overview

This system allows administrators and students to manage course registration efficiently.  
It includes:

- User authentication and role-based access (Admin / Student)  
- Viewing and registering for available courses  
- Admin dashboard to manage students and courses  
- Multi-language support (English / Arabic)  
- Email confirmation and password reset via Gmail SMTP  

---

## 🏗️ Project Structure

```
StudentCourseRegistration/
├── Controllers/              → MVC controllers
├── Data/                     → DbContext and SeedData
├── Models/                   → Entities and ViewModels
├── Repositories/             → Data access layer (Repositories)
├── Services/                 → Business logic and Email handling
├── Areas/Identity/           → Authentication and Identity pages
├── Views/                    → Razor views (UI)
├── Resources/                → Localization (ar/en)
└── wwwroot/                  → Static files (CSS, JS, Bootstrap)
```

---

## ⚙️ Requirements

Before running the project, make sure you have:

- .NET SDK 9.0 or later  
- SQL Server  
- Visual Studio or VS Code  
- A Gmail account (with App Password enabled)

---

## 🧑‍💼 Default Admin Account

You can log in as the system administrator using:

```
Email: admin@university.com
Password: Admin@123
```

---

## 📝 Setup Instructions

### 1. Configure Gmail App Password  
> ⚠️ You **cannot** use your regular Gmail password!

#### Steps:

1. **Enable 2-Step Verification:**  
   Go to [https://myaccount.google.com/security](https://myaccount.google.com/security)  
   Enable *"2-Step Verification"*.

2. **Generate App Password:**  
   Visit [https://myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords)  
   - Select **Mail** and **Windows Computer**  
   - Click **Generate**  
   - Copy the 16-character code (e.g., `abcd efgh ijkl mnop`)

3. **Update `appsettings.json`:**
   ```json
   "EmailSettings": {
     "SmtpServer": "smtp.gmail.com",
     "Port": 587,
     "SenderName": "Student Registration System",
     "SenderEmail": "example@gmail.com",
     "Password": "abcdefghijklmnop" // Remove spaces
   }
   ```

---

### 2. Initialize the Database and Run the App

Open **Package Manager Console** or **Terminal** inside the project directory and run:

```bash
# 1. Remove old migrations (optional)
Remove-Migration

# 2. Create a new migration
Add-Migration InitialCreate

# 3. Update the database
Update-Database

# 4. Run the application
dotnet run
```

Then open your browser and navigate to:
```
https://localhost:5001
```

---

## 👤 Roles & Permissions

| Role | Permissions |
|------|--------------|
| **Admin** | Manage students and courses, approve registrations |
| **Student** | View available courses, register, and manage enrolled courses |

---

## 🌐 Localization

This system supports two languages:

- 🇬🇧 English  
- 🇪🇬 Arabic  

Users can switch languages via the **LanguageController**.

---

## 📧 Email Features

Automatic email notifications are sent for:

- Account confirmation after registration  
- Password reset requests  

Implemented via:
```
Services/Implementations/EmailService.cs
```

---

## 🧠 Technologies Used

| Category | Technology |
|-----------|-------------|
| Backend | ASP.NET Core 9 (MVC) |
| ORM | Entity Framework Core |
| Authentication | Identity & Role Management |
| Database | SQL Server |
| Frontend | Razor Views, Bootstrap 5 |
| Localization | Resource Files (.resx) |
| Email | Gmail SMTP with App Password |
| Architecture | Layered Architecture (Controllers → Services → Repositories) |

---

## 🏁 Notes

- Make sure to configure your **connection string** in `appsettings.json` before running.  
- The database will be automatically created after running `Update-Database`.  
- If email sending fails, verify that your Gmail App Password is correctly configured.

---

✨ A robust, modular system designed to demonstrate real-world usage of **ASP.NET Core MVC**, **Entity Framework Core**, **Identity**, and **Email Integration**.
