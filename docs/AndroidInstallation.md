# Installing StudentSystemApp on Android Device

This guide provides step-by-step instructions to build and install the StudentSystemApp (a .NET MAUI Blazor Hybrid app) on an Android device using Visual Studio, .NET CLI, and ADB (Android Debug Bridge).

## Prerequisites

1. **Development Environment**:
   - Visual Studio 2022 (17.7+) with **.NET MAUI** workload installed.
   - Or .NET 8 SDK installed ([download](https://dotnet.microsoft.com/download/dotnet/8.0)).
   - Android SDK (included with Visual Studio or via Android Studio).

2. **Android Device**:
   - Android device with API level 21+ (Android 5.0+).
   - USB debugging enabled:
     - Go to **Settings > About phone** → Tap **Build number** 7 times to enable Developer options.
     - Go to **Settings > Developer options** → Enable **USB debugging**.
   - USB cable for connection.

3. **ADB (Android Debug Bridge)**:
   - Install Android SDK Platform-Tools:
     - [Download from Google](https://developer.android.com/tools/releases/platform-tools).
     - Extract to a folder, e.g., `C:\\android-sdk\\platform-tools`.
     - Add to PATH: `C:\\android-sdk\\platform-tools` (restart terminal/VS after).

## Method 1: Using Visual Studio (Recommended)

1. Open `StudentSystemApp.sln` in Visual Studio.

2. Connect Android device via USB (authorize if prompted).

3. In toolbar:
   - Select **Debug** target.
   - Select your **Android device** from the device dropdown.

4. Ensure **StudentSystemApp** (Android target) is startup project.

5. Press **F5** or click **Run** to build and deploy.

The app installs and launches automatically.

## Method 2: Using .NET CLI + ADB

1. Open terminal in project root (`c:/Users/My PC/OneDrive/Desktop/Dev/StudentSystem/StudentSystemApp`).

2. Connect device and verify with ADB:
   ```
   adb devices
   ```
   - Output should list your device (e.g., `ABC1234 device`). Authorize if prompted.

3. Publish APK:
   ```
   dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=apk --no-restore
   ```
   - APK generated at: `bin\\Release\\net8.0-android\\publish\\StudentSystemApp.apk`.

4. Install via ADB:
   ```
   adb install bin\\Release\\net8.0-android\\publish\\StudentSystemApp.apk
   ```

5. Launch app:
   ```
   adb shell am start -n com.companyname.studentsystemapp/com.companyname.studentsystemapp.MainActivity
   ```
   - Package name from `Platforms\\Android\\AndroidManifest.xml`.

## Troubleshooting

- **Device not detected**:
  - Run `adb kill-server && adb start-server`.
  - Check USB drivers (install Google USB Driver if needed).
  - Try different USB cable/port.

- **Build errors**:
  - Ensure Android SDK paths set in VS: **Tools > Options > Xamarin > Android Settings**.
  - Clean/Rebuild: `dotnet clean && dotnet build`.

- **App crashes**:
  - Check logs: `adb logcat`.
  - Ensure .NET 8 runtime for Android if using AOT.

- **Multiple devices**:
  - Specify device: `adb -s ABC1234 install app.apk`.

## Uninstall

```
adb uninstall com.companyname.studentsystemapp
```

For production, generate AAB for Play Store:
```
dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=aab
```

