# Installing StudentSystemApp on Android Device

This guide provides step-by-step instructions to build and install the StudentSystemApp (.NET MAUI Blazor Hybrid, net9.0-android) on an Android device using Visual Studio, .NET CLI, and ADB.

## Prerequisites

1. **Development Environment**:
   - Visual Studio 2022 with .NET MAUI workload.
   - .NET 9 SDK + workloads:
     ```
     dotnet workload install maui-android --source https://aka.ms/dotnet9/nuget/index.json
     dotnet workload restore
     ```
     ([.NET 9 download](https://dotnet.microsoft.com/download/dotnet/9.0)).
   - Android SDK (VS or Android Studio).

2. **Android Device**:
   - API 21+.
   - USB debugging enabled (Settings > Developer options).

3. **ADB**:
   - Download Platform-Tools, add `platform-tools` to PATH.

## Method 1: Visual Studio (Recommended)

1. Open `StudentSystemApp.sln`.
2. Connect device.
3. Select Android device & net9.0-android target.
4. F5 to deploy.

## Method 2: .NET CLI + ADB

1. Terminal in project root.
2. `adb devices` (authorize).

3. **Restore & Publish APK (Release)**:
   ```
   dotnet restore --source https://aka.ms/dotnet9/nuget/index.json
   dotnet publish -f net9.0-android -c Release -p:AndroidPackageFormat=apk -r android-arm64
   ```
   - APK: `bin/Release/net9.0-android/android-arm64/publish/StudentSystemApp.apk`
   - **Signed APK** (recommended): `bin/Release/net9.0-android/android-arm64/com.companyname.studentsystemapp-Signed.apk`
   - RID: android-arm64 (phone) or android-x64 (tablet).

4. **Install**:
   ```
   adb install "bin/Release/net9.0-android/android-arm64/com.companyname.studentsystemapp-Signed.apk"
   ```
   (Or unsigned: replace with StudentSystemApp.apk).

5. **Launch**:
   ```
   adb shell am start -n com.companyname.studentsystemapp/com.companyname.studentsystemapp.MainActivity
   ```

## Troubleshooting

- **Restore fails**: Run restore with preview source above.
- **Platform error**: csproj TargetFrameworkVersion=34.0.
- **Device**: `adb kill-server && adb start-server`.
- Logs: `adb logcat`.
- Clean: `dotnet clean`.

## Uninstall
```
adb uninstall com.companyname.studentsystemapp
```

**Production AAB**:
```
dotnet publish -f net9.0-android -c Release -p:AndroidPackageFormat=aab -r android-arm64
```

