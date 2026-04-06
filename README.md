# Student System App

## Overview
A cross-platform .NET MAUI Blazor Hybrid app for managing student information using SQLite database.

**Technologies:**
- .NET 9.0 MAUI (Android, iOS, MacCatalyst, Windows)
- Blazor WebView (hybrid native/web)
- Dapper ORM
- Microsoft.Data.Sqlite

## Features
- Cross-platform student list (`/students`) with SQLite data
- **CREATE new student** (`/students/create`) with validation + Dapper INSERT
- Responsive Bootstrap UI
- Blazor routing + EditForm/DataAnnotations

## CRUD Implementation Notes
**Dapper INSERT SQL:**
```sql
INSERT INTO Students (StudentNumber, FullName, Course) 
VALUES (@StudentNumber, @FullName, @Course);
SELECT last_insert_rowid();
```
**Flow:** EditForm → Validation → Repo.CreateAsync() → Redirect to list

## Project Structure
```
StudentSystemApp/
├── Components/           # Blazor Razor components
│   ├── Layout/          # Shared layout (MainLayout, NavMenu)
│   ├── Pages/           # Page components (Home, Students, Counter)
│   ├── Routes.razor     # App router
│   └── _Imports.razor   # Global usings
├── Models/              # Data models (Student.cs)
├── Services/            # Repository pattern (StudentRepository.cs)
├── MauiProgram.cs       # App builder & DI registration
├── Resources/           # Images, icons, fonts
├── Platforms/           # Platform-specific code
├── wwwroot/             # Static web assets (CSS, JS)
└── StudentSystemApp.csproj  # Multi-target MAUI project
```

## Setup & Run
1. **Prerequisites:**
   ```
   .NET 9 SDK
   Android SDK (Visual Studio or VSCode Android extension)
   Windows 10 SDK (for Windows target)
   Xcode (for iOS/Mac, macOS only)
   ```

2. **Restore & Build:**
   ```bash
   dotnet restore
   dotnet build -c Release
   ```

3. **Run Targets:**

   **Windows Desktop App (Blazor Hybrid):**
   ```bash
   dotnet run --framework net9.0-windows10.0.19041.0
   ```
   *Opens native Windows app with embedded Blazor WebView*

   **Android Emulator:**
   ```bash
   dotnet run --framework net9.0-android
   ```
   *Requires Android SDK/emulator + `dotnet workload install maui-android`*

   **Pure Blazor Web (Standalone WebAssembly):**
   1. Create `wwwroot/appsettings.json`:
      ```
      {"ConnectionStrings":{"DefaultConnection":"Data Source=students-web.db"}}
      ```
   2. Run:
      ```bash
      dotnet publish -c Release -r win-x64 --self-contained false -p:PublishProfile=FolderProfile /p:PublishDir=publish-web
      ```
   3. Open `publish-web/wwwroot/index.html` in browser.

   **Full clean build:**
   ```bash
   dotnet clean && dotnet restore && dotnet build && dotnet run --framework net9.0-windows10.0.19041.0
   ```

4. **Navigation:**
   - Home: `/`
   - Students: `/students` (SQLite data table)
   - Counter: `/counter` (demo)

## Database
- **File:** `students.db` (generated in app dir)
- **Initialization:** `DbInitializer.cs` seeds 4 sample students on first run
- **Repository:** `StudentRepository.cs` uses Dapper for CRUD

Sample data:
| ID | Student # | Name | Course |
|----|-----------|--------------|---------|
| 1 | S001 | John Doe | Computer Science |
| 2 | S002 | Jane Smith | Mathematics |

## Key Learnings (Education)
1. **MAUI Blazor Hybrid:** Native shell + Blazor WebView (single codebase, multi-platform)
2. **Dependency Injection:** Register services in `MauiProgram.cs` (`builder.Services.AddScoped<StudentRepository>()`)
3. **Repository Pattern:** `StudentRepository` abstracts SQLite access with Dapper
4. **Blazor Routing:** `@page "/path"`, `<Router>`, `<NavLink>` (resolve ambiguities)
5. **SQLite Integration:** Async DB ops, connection scoping, migrations/seed
6. **Platform Targets:** Multi-target frameworks, conditional compilation
7. **Hot Reload:** Blazor XAML source gen (`MauiXamlInflator=SourceGen`)

## Troubleshooting
| Issue | Solution |
|-------|----------|
| csproj MSB4025 | Remove BOM/leading chars (UTF8 no BOM) |
| Ambiguous routes | Unique `@page` directives |
| DbContext null | DI registration + scoped lifetime |
| Build fails Android | Install Android workloads: `dotnet workload install maui-android` |

## Next Steps (Extensions)
- Add CRUD forms (Create/Edit/Delete students)
- Search/filter/sort table
- Charts/graphs (MudBlazor charts)
- Authentication (MAUI SecureStorage)
- Export to CSV/PDF
- Push notifications (MAUI Essentials)

## Credits
**Developed by Pio Leonardo Rapirap**

Generated with MAUI Blazor template, enhanced with SQLite + Dapper for full CRUD by Pio Leonardo Rapirap.
