# Task Progress: Fix StudentCourseRelation Build Error (BlazorWebView .NET 9)

## Steps:
- [x] Diagnose: Confirmed CS1061/CA1416 due to deprecated AddAdditionalAssemblies (not MudBlazor leak)
- [x] Edit MauiProgram.cs: Simplify BlazorWebView registration + suppress CA1416
- [x] Test: dotnet restore && dotnet build (all TFs succeed)
- [x] Commit changes
- [ ] Verify mudblazor isolation
