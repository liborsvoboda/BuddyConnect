﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace BuddyConnect;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle savedInstanceState) {
        base.OnCreate(savedInstanceState);
        if (Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.R && ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.BluetoothConnect) != Permission.Granted) {
            ActivityCompat.RequestPermissions(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, new string[] { Android.Manifest.Permission.BluetoothConnect }, 102);
        }
        if (Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.R && ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.Bluetooth) != Permission.Granted) {
            ActivityCompat.RequestPermissions(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, new string[] { Android.Manifest.Permission.Bluetooth }, 102);

        }
    }
}

