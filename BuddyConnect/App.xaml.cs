using Plugin.BLE;
using SQLite;

namespace BuddyConnect;

public partial class App : Application {


	//Central Declaration
    public static AppSetting appSetting;


    public App(IServiceProvider provider) {

        InitializeComponent();
        
        appSetting = new AppSetting();
        MainPage = new AppShell();

        //Service Provider
        App.appSetting.ServiceProvider = provider;

        //BlueTooth Init
        App.appSetting.BlueTooth = new BlueTooth();
        App.appSetting.BlueTooth.Bluetooth = CrossBluetoothLE.Current;
        App.appSetting.BlueTooth.BtAdapter = CrossBluetoothLE.Current.Adapter;

    }
}
