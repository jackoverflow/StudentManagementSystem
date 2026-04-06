# Student Guide: Understanding the StudentSystem App

**Yes!** This guide is written for beginner/intermediate students. Follow step-by-step to understand **every part**.

> [!IMPORTANT]
> **Which version am I on?** 
> This app has two versions. You are currently on **Version 2 (Relational)**. 
> Unlike Version 1, which used simple text for courses, this version uses a professional database setup with IDs and Links (Foreign Keys).
>
> **Switching Versions via Terminal:**
> *   Switch to Simple Version: `git checkout StudentManagementSystem`
> *   Switch to Relational Version: `git checkout StudentCourseRelation`
>
> **Smart Branch Switching:**
> We have added "Self-Healing" code in `DbInitializer.cs`. 
> If you switch back to this branch from the Relational one, the app will notice if the `Course` column is missing 
> and fix the database for you automatically. 
> 
> *No more "No such column: Course" errors!*

## 1. The Big Picture
```
Native App (MAUI) ← Embeds → Blazor Web UI (Components/*.razor) ← Talks to → SQLite DB (students.db)
                    ↑ DI Container (MauiProgram.cs)
```

**Analogy**: Like a restaurant - Native shell (building), Blazor (waiters/menu), Repo/DB (kitchen/storage).

## 2. Start Here: README.md
Run the app, see features, setup.

## 3. Core Files (Read in Order)
```
1. Models/Student.cs - "What data looks like"
2. Services/StudentRepository.cs - "Database chef (CRUD methods)"
3. Components/Pages/DbInitializer.cs - "Kitchen setup (create table + sample food)"
4. MauiProgram.cs - "Restaurant manager (registers services)"
5. Components/Pages/Students.razor - "Main dining room (table + actions)"
6. Components/Pages/CreateStudent.razor - "Order form"
```

## 4. How App Starts (Timeline)
1. `dotnet run` → `MauiProgram.CreateMauiApp()`
2. Calls `DbInitializer.Initialize()` → creates `students.db` + 4 sample students
3. Registers `StudentRepository` as Singleton (one instance)
4. Loads `/students` → `OnInitializedAsync()` → `repo.GetAllAsync()` → render table
5. User clicks DELETE → `DeleteStudent(id)` → `repo.DeleteAsync(id)` → `LoadStudents()` → table re-renders

## 5. Key Concepts Explained Simply

**@inject StudentRepository repo**
- Like "give me the kitchen access card"

**async Task GetAllAsync()**
- "Go to kitchen async (non-blocking), get all food list"

**StateHasChanged()**
- "Update the menu display NOW"

**@page "/students"**
- "This room is /students"

**EditForm Model="student"**
- "Fill form with this data object"

## 6. Debug Like Pro
```
F12 → Console tab → Try delete → See "Deleted [guid], reloaded 3 students"
No log? Check try-catch error.
Table not update? Check StateHasChanged calls.
```

## 7. Why "Ghost" Delete Fixed
```
Delete works → Record gone from DB
App restart → DbInitializer adds samples AGAIN → "Ghost" appears
Solution: del students.db → Fresh DB
```

## 8. Quiz Yourself
1. Where is SQL written? (Repo)
2. What happens without StateHasChanged? (No UI update)
3. Why Singleton repo? (One DB connection)
4. Lifecycle for data load? (OnInitializedAsync)

## 9. Extend It (Homework)
1. Add search box
2. Add "Course count" chart
3. Make DbInitializer optional (button)

**Mastery Check**: Explain to friend how delete button → SQL → UI refresh.

Perfect for CS students/portfolio!
