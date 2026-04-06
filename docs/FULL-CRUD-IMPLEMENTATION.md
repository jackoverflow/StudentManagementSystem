# Full CRUD Implementation Guide

## Complete Student Management System

**Status**: ✅ Full CRUD operations implemented (Create, Read, Update, Delete).

### CRUD Operations Explained

**1. READ (List All)**
```
Route: /students
Flow: OnInitializedAsync() → StudentRepo.GetAllAsync() → @foreach render table
SQL: SELECT Id, StudentNumber, FullName, Course FROM Students
Blazor: Conditional rendering (loading → data → empty)
```

**2. CREATE (Add New)**
```
Route: /students/create
Flow: EditForm → Validation (DataAnnotations) → StudentRepo.CreateAsync() → NavigateTo(\"/students\")
SQL: INSERT INTO Students (...) VALUES (...) RETURNING last_insert_rowid()
Model Binding: @bind-Value two-way binding
```

**3. UPDATE (Edit)**
```
Route: /students/edit/{Id}
Flow: OnParametersSetAsync() → StudentRepo.GetByIdAsync(id) → populate form → Submit → UpdateAsync → NavigateTo list
SQL: SELECT * FROM Students WHERE Id = @Id
SQL: UPDATE Students SET ... WHERE Id = @Id
Id readonly (hidden or disabled)
```

**4. DELETE (Remove)**
```
Row Button: @onclick=\"() => DeleteStudent(student.Id)\"
Flow: JS confirm → StudentRepo.DeleteAsync(id) → LoadStudents() → StateHasChanged() refresh
SQL: DELETE FROM Students WHERE Id = @Id
UI: isDeleting spinner + disable buttons during op
Troubleshoot: DbInitializer repopulates on restart
```

### Code Flow Diagram
```
App Start → MauiProgram → DbInitializer → Sample Data
User → /students → GetAllAsync → Render Table
User clicks DELETE → JS Confirm → DeleteAsync → LoadStudents → Re-render
```


### Architecture Highlights
1. **Repository Pattern**: `StudentRepository.cs` - Dapper + async SQLite CRUD
2. **Dependency Injection**: `MauiProgram.cs` - `AddSingleton<StudentRepository>()`
3. **Blazor Forms**: `EditForm` + `DataAnnotations` validation
4. **UI Feedback**: Loading spinners, JS confirm, `StateHasChanged()` refresh
5. **Lifecycle**: `OnInitializedAsync` → `LoadStudents()` → table render

### Troubleshooting Delete "Ghost" Records
**Cause**: `DbInitializer.Initialize()` runs on **every app start**, recreating sample data. Delete removes correctly, but restart repopulates.

**Fixes Implemented**:
```
- Enhanced LoadStudents() + StateHasChanged()
- Visible spinner during delete/loading  
- Console logging: \"Deleted [id], reloaded X students\"
- DB reset: del students.db → fresh init
```

### Educational Extensions
1. **Search/Filter**: Add `@bind-Value` search input → LINQ `students.Where(...)`
```csharp
private string searchTerm = "";
// In LoadStudents after assign:
students = students.Where(s => s.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
```
2. **Sorting**: Clickable headers → `students = students.OrderByDescending(...)`
3. **Pagination**: MudBlazor `<MudTable ServerData>` 
4. **Error UI**: Replace `Console.WriteLine` → MudBlazor Snackbar
5. **Auth**: MAUI SecureStorage + `[Authorize]` attribute
6. **Export**: `CsvHelper` + download button
7. **Charts**: MudBlazor `<MudChart>` course stats

### Production Polish
```
dotnet workload install maui-android  # Full mobile support
Add-Migration InitialCreate           # EF migrations (optional)
Add NuGet: MudBlazor                 # Advanced UI components
```

### Run All Platforms
```bash
# Windows
dotnet run -f net9.0-windows10.0.19041.0

# Android (emulator)
dotnet run -f net9.0-android

# iOS/Mac (macOS only)
dotnet run -f net9.0-maccatalyst
```

**App Ready for Deployment!** 🚀

**Key Learning**: Hybrid apps perfectly blend web productivity (Blazor UI) with native power (SQLite local storage).
