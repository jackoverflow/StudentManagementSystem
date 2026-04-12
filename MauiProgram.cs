﻿using Microsoft.Extensions.Logging;
using StudentSystemApp.Services;

namespace StudentSystemApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        // Initialize the SQLite database
        StudentSystemApp.Components.Pages.DbInitializer.Initialize();
        builder.Services.AddSingleton<StudentRepository>();
        builder.Services.AddSingleton<CourseRepository>();

builder.Services.AddMauiBlazorWebView();

#pragma warning disable CA1416 // Suppress Android platform warning (safe for app)

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

#pragma warning restore CA1416

		return builder.Build();
	}
}
