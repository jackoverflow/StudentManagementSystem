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

		#if ANDROID23_0_OR_GREATER
		builder.Services.AddMauiBlazorWebView();
		#else
		builder.Services.AddMauiBlazorWebView().AddAdditionalAssemblies(typeof(Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView).Assembly);
		#endif

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
