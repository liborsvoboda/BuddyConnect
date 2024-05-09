using BuddyConnect.Resources.Languages;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace BuddyConnect;

public partial class MainPage : ContentPage, GlobalServices {
    public BlueTooth test = App.appSetting.BlueTooth;


    public MainPage() {

        InitializeComponent();
        _ = HasCorrectPermissions();

        ConfigureBLE();
        _ = LoadStartUpData();
    }

    // Set up scanner
    private void ConfigureBLE() {
        App.appSetting.BlueTooth.BtAdapter.ScanMode = ScanMode.LowLatency;
        App.appSetting.BlueTooth.BtAdapter.ScanTimeout = 10000; // ms
        App.appSetting.BlueTooth.Bluetooth.StateChanged += Bluetooth_StateChanged;
        App.appSetting.BlueTooth.BtAdapter.ScanTimeoutElapsed += BtAdapter_ScanTimeoutElapsed;
        App.appSetting.BlueTooth.BtAdapter.DeviceAdvertised += BtAdapter_DeviceAdvertised; ;
        App.appSetting.BlueTooth.BtAdapter.DeviceDiscovered += BtAdapter_DeviceDiscovered; ;
    }

    private void BtAdapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
       // App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
    }

    //Detect Devices By Name
    private void BtAdapter_DeviceAdvertised(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
        if (e.Device.Name != null && e.Device.Name.ToLower().Contains(App.appSetting.DeviceName.ToLower())
            && (App.appSetting.BlueTooth.BtAvailableDevices.Count == 0 || (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0 && !App.appSetting.BlueTooth.BtAvailableDevices.Contains(e.Device)))) {
            App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
        }
    }

    private void BtAdapter_ScanTimeoutElapsed(object sender, EventArgs e) {
        if (App.appSetting.BlueTooth.BtAvailableDevices.Count == 0) {
            bt_status.Text = AppResources.ResourceManager.GetString("ScanTimeout", new CultureInfo(App.appSetting.Language));
        }
    }
    

    private void Bluetooth_StateChanged(object sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e) {
        LoadStartUpData();
    } 

    private async Task<bool> HasCorrectPermissions() {
        var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
        if (permissionResult != PermissionStatus.Granted) {
            permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
        }
        if (permissionResult != PermissionStatus.Granted) {
            AppInfo.ShowSettingsUI();
            return false;
        }
        return true;
    }

    public bool LoadStartUpData() {

        try {
            if (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0 && App.appSetting.BlueTooth.BtAvailableDevices[0].State.ToString().ToLower() == "connected") {
                bt_status.Text = AppResources.ResourceManager.GetString("Connected", new CultureInfo(App.appSetting.Language));
            }
            else if (!App.appSetting.BlueTooth.Bluetooth.IsAvailable) {
                bt_status.Text = AppResources.ResourceManager.GetString("NotAvailable", new CultureInfo(App.appSetting.Language));
                bt_button.IsVisible = false;  //App.appSetting.BlueTooth.BtAvailableDevices[0].State
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "off") {
                bt_status.Text = AppResources.ResourceManager.GetString("Off", new CultureInfo(App.appSetting.Language));
                bt_button.IsVisible = false;
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "on") {
                bt_status.Text = AppResources.ResourceManager.GetString("On", new CultureInfo(App.appSetting.Language));
                bt_button.IsVisible = true;
            }

        } catch { }
        aiLoading.IsRunning = false;
        return true;
    }

    public async Task Dismiss() { await Navigation.PopModalAsync(); }


    private async void BtButton_Clicked(object sender, EventArgs e) {
        try {

            aiLoading.IsRunning = true;
            //App.appSetting.BlueTooth.BtDevices.Clear();
            //App.appSetting.BlueTooth.BtDevices[0].UpdateRssiAsync();
            // App.appSetting.BlueTooth.BtAdapter.DeviceDiscovered += (s, a) => App.appSetting.BlueTooth.BtDevices.Add(a.Device);

            //ScanFilterOptions scanFilterOptions = new ScanFilterOptions();scanFilterOptions.DeviceAddresses = new[] { "dbd00001-ff30-40a5-9ceb-a17358d31999", "device_information" };
            await App.appSetting.BlueTooth.BtAdapter.StartScanningForDevicesAsync();
            test = App.appSetting.BlueTooth;

            if (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0) {

                App.appSetting.Devices.Clear();

                App.appSetting.BlueTooth.BtAvailableDevices.ForEach(availableDevice => {

                    Debug.WriteLine(availableDevice);

                    //Nactena existence Jednotky Name + Adresa 
                    //[0:] [{"Id":0,"Name":"DVBdiver@DVB11_06","Address":"80:E1:26:3A:CB:16","Timestamp":"2024-05-08T21:09:09.1210524+02:00"}]
                    App.appSetting.Devices.Add(new DatabaseModel.DeviceList() { Name = availableDevice.Name, Address = availableDevice.NativeDevice.ToString() });
                    Debug.WriteLine(JsonSerializer.Serialize<object>(App.appSetting.Devices));


                    var textCell = new TextCell() { Text = availableDevice.Name, };
                    textCell.Tapped += DeviceTable_Clicked; deviceList.Add(textCell);

                    //Nacteni dat
                    //[0:][{ "Type":1,"Data":"Bg=="},{ "Type":25,"Data":"QBQ="},{ "Type":9,"Data":"RFZCZGl2ZXJARFZCMTFfMDY="}]
                    Debug.WriteLine(JsonSerializer.Serialize<object>(availableDevice.AdvertisementRecords));

                    
                    
                });

                lb_none.IsVisible = false; deviceTable.IsVisible = true;
            } else { deviceTable.IsVisible = false; lb_none.IsVisible = true; }

        } catch { }

        aiLoading.IsRunning = false;
        LoadStartUpData();
        
    }

    //Load Data From Founded Device
    private async void DeviceTable_Clicked(object sender, EventArgs e) {
        try {
            aiLoading.IsRunning = true;

            await App.appSetting.BlueTooth.BtAdapter.ConnectToDeviceAsync(App.appSetting.BlueTooth.BtAvailableDevices.First(a => a.Name.ToString() == ((TextCell)sender).Text));
            var aha = App.appSetting.BlueTooth.BtAvailableDevices[0].State;

            Guid frstGuid = App.appSetting.BlueTooth.BtAvailableDevices[0].Id;
            BlueTooth nove = (BlueTooth)App.appSetting.BlueTooth.BtAvailableDevices[0].NativeDevice;
            //nove

            var mozna = await App.appSetting.BlueTooth.BtAvailableDevices[0].GetServicesAsync();
            var mozna1 = await mozna[0].GetCharacteristicsAsync();
            var final = JsonSerializer.Serialize<object>(mozna1[0].StringValue); 

            var guid = mozna[0].Id;
            var dalsi = await mozna[0].GetCharacteristicAsync(guid);


            var navrat = await mozna[1].GetCharacteristicsAsync();

            //read characreristic from device
            var download = await navrat[0].ReadAsync();
            //navrat vraci 3 zaznamy


            //service read/write/update/Uuid/Value/stringValue Name
            var neco = await dalsi.ReadAsync();



            var dal = await App.appSetting.BlueTooth.BtAvailableDevices[0].GetServiceAsync(guid);

            //dal1 nabizi Write / update / read
            var dal1 = await dal.GetCharacteristicAsync(guid);
            // havaruje   var dal2 = await dal1.ReadAsync();
            var dal3 = await dal1.GetDescriptorAsync(guid);
            var dal4 = await dal3.ReadAsync();

            var mozna3 = mozna1[0].Properties;
            var mozna4 = await mozna1[0].GetDescriptorsAsync();
            var mozna5 = await mozna4[0].ReadAsync();

        } catch (Exception ex) { }
        aiLoading.IsRunning = false;
        LoadStartUpData();
    }


}

