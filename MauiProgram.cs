﻿using Microsoft.Extensions.Logging;
using MudBlazor.Services;
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
		builder.Services.AddMudServices();


		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
