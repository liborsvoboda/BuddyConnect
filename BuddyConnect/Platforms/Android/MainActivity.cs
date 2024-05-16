using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace BuddyConnect;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected async override void OnCreate(Bundle savedInstanceState) {
        base.OnCreate(savedInstanceState);
        if (Build.VERSION.SdkInt > BuildVersionCodes.R && ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.BluetoothConnect) != Permission.Granted) {
            ActivityCompat.RequestPermissions(Platform.CurrentActivity, new string[] { Android.Manifest.Permission.BluetoothConnect }, 102);
        }
        if (Build.VERSION.SdkInt <= BuildVersionCodes.R && ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.Bluetooth) != Permission.Granted) {
            ActivityCompat.RequestPermissions(Platform.CurrentActivity, new string[] { Android.Manifest.Permission.Bluetooth }, 102);

        }

        await Permissions.RequestAsync<BluetoothLEPermissions>();
    }
}

public class BluetoothLEPermissions : Permissions.BasePlatformPermission {
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions {
        get {
            return new List<(string androidPermission, bool isRuntime)>
            {

                (Android.Manifest.Permission.Bluetooth, true),
                (Android.Manifest.Permission.BluetoothAdmin, true),
                (Android.Manifest.Permission.BluetoothScan, true),
                (Android.Manifest.Permission.BluetoothConnect, true),
                (Android.Manifest.Permission.AccessFineLocation, true),
                (Android.Manifest.Permission.AccessCoarseLocation, true),
                (Android.Manifest.Permission.AccessBackgroundLocation, true),

            }.ToArray();
        }
    }
}

