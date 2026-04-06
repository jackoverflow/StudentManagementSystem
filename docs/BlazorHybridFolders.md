# .NET MAUI Blazor Project Structure

Welcome to the world of .NET MAUI Blazor! It's a powerful way to build cross-platform apps using web technologies while still having full access to native device capabilities.

Since you've just set up your project, understanding these two folders is key because they represent the "web" heart of your hybrid application. Here is a breakdown of their purposes:

This document explains the purpose of the key directories in a .NET MAUI Blazor Hybrid application.

## 1. The `wwwroot` Folder
The `wwwroot` folder is the **web root** of the application. It behaves similarly to the web root in standard ASP.NET Core or static website projects.

*   **Static Assets**: This is the storage location for all static web files, such as CSS stylesheets, JavaScript files, images, and fonts.
*   **Entry Point**: It typically contains the `index.html` file, which serves as the host page where the Blazor application is rendered within the native MAUI container.
*   **Accessibility**: Anything placed in this folder is accessible via a relative path. For example, a file at `wwwroot/css/app.css` is referenced in HTML as `css/app.css`.

## 2. The `Components` Folder
The `Components` folder is where the **UI logic and markup** reside. In a Blazor Hybrid app, you primarily build your interface using Razor components instead of XAML.

*   **Razor Components**: Each `.razor` file is a reusable piece of UI. These components combine HTML for structure and C# for logic.
*   **Layouts**: Usually found in a `Layout` subfolder, these contain shared components like `MainLayout.razor` (the app wrapper) and `NavMenu.razor`.
*   **Pages**: Usually found in a `Pages` subfolder, these define the different "screens" of your app using the `@page` directive.
*   **Native Interop**: Because these components run within the MAUI process, they can directly call native APIs and services.

## How They Work Together
When the MAUI app starts, it initializes a `BlazorWebView`. This control points to the `index.html` file inside `wwwroot`. The `index.html` file then bootstraps the Blazor framework, which begins rendering the components defined in the `Components` folder. 

Think of `Components` as the **application logic and structure** and `wwwroot` as the **static resources** that support that structure.