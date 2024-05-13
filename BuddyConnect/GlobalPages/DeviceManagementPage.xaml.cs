using BuddyConnect.Controllers;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;
using Microsoft.Maui.Controls;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using System.Globalization;


namespace BuddyConnect;

public partial class DeviceManagementPage : ContentPage, GlobalServices {


    public BlueTooth test = App.appSetting.BlueTooth;

    public DeviceManagementPage() {

        InitializeComponent();
        _ = CheckBtPermissions();

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

    private async void BtAdapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
        // App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
        await LoadStartUpData();
    }

    //Detect Devices By Name
    private void BtAdapter_DeviceAdvertised(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
        if (e.Device.Name != null && e.Device.Name.ToLower().Contains(App.appSetting.DeviceName.ToLower())
            && (App.appSetting.BlueTooth.BtAvailableDevices.Count == 0 || (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0 && !App.appSetting.BlueTooth.BtAvailableDevices.Contains(e.Device)))) {
            App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
        }
    }

    //ScanTimeout Action
    private async void BtAdapter_ScanTimeoutElapsed(object sender, EventArgs e) => await LoadStartUpData();
    //BT State Changed Action
    private async void Bluetooth_StateChanged(object sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e) => await LoadStartUpData();

    //Check BT Permissions
    private async Task<bool> CheckBtPermissions() {
        var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
        if (permissionResult != PermissionStatus.Granted) { permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>(); }
        if (permissionResult != PermissionStatus.Granted) { AppInfo.ShowSettingsUI(); return false; }
        return true;
    }

    //Set BT Device Status
    public async Task<bool> LoadStartUpData() {

        try {
            if (App.appSetting.BlueTooth.BtAdapter.ConnectedDevices.Count > 0) {
                lbl_status.Text = AppResources.Connected;
            }
            else if (!App.appSetting.BlueTooth.Bluetooth.IsAvailable) {
                lbl_status.Text = AppResources.NotAvailable;
                bt_scanButton.IsVisible = false;
                /*App.appSetting.Devices.Clear();*/ deviceList.Clear(); infoList.Clear();
                bt_infoButton.IsVisible = bt_filesButton.IsVisible = false;
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "off") {
                lbl_status.Text = AppResources.Off;
                bt_scanButton.IsVisible = false;
                /*App.appSetting.Devices.Clear();*/ deviceList.Clear(); infoList.Clear();
                bt_infoButton.IsVisible = bt_filesButton.IsVisible = false;
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "on") {
                lbl_status.Text = AppResources.On;
                bt_scanButton.IsVisible = true;
            }
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        return true;
    }

    public async Task Dismiss() { await Navigation.PopModalAsync(); }

    //Disconnect and Clear Datasets
    private async Task<bool> DisconnectBTDevices() {
        foreach (var device in App.appSetting.BlueTooth.BtAdapter.ConnectedDevices) { await App.appSetting.BlueTooth.BtAdapter.DisconnectDeviceAsync(device); }
        App.appSetting.BlueTooth.BtAvailableDevices.Clear(); App.appSetting.BlueTooth.Characteristics.Clear();
        //App.appSetting.Services.Clear(); 
        ClearBTdata();
        return true;
    }

    //Clear Readed BTdata 
    private void ClearBTdata() {
        /*App.appSetting.Devices.Clear(); */deviceList.Clear(); infoList.Clear();
        App.appSetting.DeviceInfoList.Clear(); App.appSetting.DeviceFileLists.Clear();
    }

    //Set Indicator IsLoading
    private void IsLoading(bool isLoading) {
        if (isLoading) { //((Button)button).BackgroundColor = Color.Parse("Red");
            PageContent.Children.Where(a => a.GetType().Name == "Button").ToList().ForEach(button => { ((Button)button).IsEnabled = false; });
        } else { PageContent.Children.Where(a => a.GetType().Name == "Button").ToList().ForEach(button => { ((Button)button).IsEnabled = true; }); }
        aiLoading.IsRunning = isLoading;
    }

    //Run BT Scanning 
    private async void BtScanButton_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true);
            bt_infoButton.IsVisible = bt_filesButton.IsVisible = false;

            //Disconect All 
            await DisconnectBTDevices();

            await App.appSetting.BlueTooth.BtAdapter.StartScanningForDevicesAsync();
            if (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0) {

                App.appSetting.BlueTooth.BtAvailableDevices.ForEach(availableDevice => {
                    //App.appSetting.Devices.Add(new DeviceList() { Name = availableDevice.Name, Address = availableDevice.NativeDevice.ToString() });
                    var textCell = new TextCell() { Text = availableDevice.Name }; textCell.Tapped += SelectDevice_Clicked; deviceList.Add(textCell);
                });
            }
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false); bt_scanButton.IsEnabled = true;
        await LoadStartUpData();
    }

    //Load Data From Founded Device
    private async void SelectDevice_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true); ClearBTdata(); infoList.Title = AppResources.InfoReview;

            //Connect To BT Device RUN THIS DUAL CONNECTION ONLY
            var adapter = App.appSetting.BlueTooth.BtAdapter;
            adapter?.ConnectToDeviceAsync(App.appSetting.BlueTooth?.BtAvailableDevices?.First(a => a.Name.ToString() == ((TextCell)sender).Text), default);
            IReadOnlyList<IDevice> connectedDevice = adapter.GetKnownDevicesByIds(new Guid[] { (Guid)App.appSetting.BlueTooth?.BtAvailableDevices?.First(a => a.Name.ToString() == ((TextCell)sender).Text).Id });
            foreach (var cdevice in connectedDevice) { await adapter.ConnectToDeviceAsync(cdevice); }

            //Get Connected Device
            var device  = App.appSetting.BlueTooth.BtAdapter.ConnectedDevices?.ToList()[0];

            IReadOnlyList<IService> services = await device.GetServicesAsync(default);
            foreach (var service in services) {
                //Load CHaracteristics And Info Values
                IReadOnlyList<ICharacteristic> characteristics = await service.GetCharacteristicsAsync();
                foreach (ICharacteristic characteristic in characteristics) {
                    App.appSetting.BlueTooth.Characteristics.Add(new BtCharacteristics() { Name = characteristic?.Name, UUid = characteristic.Id.ToString(), Characteristic = characteristic });
                    
                    if (App.appSetting.CharDeviceInfoDefLists.Where(a => a.CharName == characteristic?.Name && a.Uuid == null).Count() > 0) {
                        //Insert Standard BT Info by Name
                        string value = System.Text.Encoding.UTF8.GetString((await characteristic.ReadAsync()).data);
                        App.appSetting.DeviceInfoList.Add(new DeviceInfoList() { Name = characteristic?.Name, Value = value });
                        infoList.Add(new TextCell() { Text = characteristic?.Name, Detail = value });

                    } else if (App.appSetting.CharDeviceInfoDefLists.Where(a => a.Uuid != null && a.Uuid.ToLower() == characteristic?.Uuid.ToString().ToLower()).Count() > 0) {
                        //Insert DVB info by Uuid
                        CharDeviceInfoDefList customInfo = App.appSetting.CharDeviceInfoDefLists.Where(a => a.Uuid != null && a.Uuid.ToLower() == characteristic?.Uuid.ToString().ToLower()).FirstOrDefault();
                         
                        string value = System.Text.Encoding.UTF8.GetString((await characteristic.ReadAsync()).data);
                        App.appSetting.DeviceInfoList.Add(new DeviceInfoList() { Name = customInfo.CharName, Value = value });
                        infoList.Add(new TextCell() { Text = customInfo.CharName, Detail = value });
                    }
                }
            }

            //Show Action Buttons
            bt_infoButton.IsVisible = bt_filesButton.IsVisible = true;
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false);
        await LoadStartUpData();
    }



    //Show Loaded Info
    private void BtnInfoButton_Clicked(object sender, EventArgs e) {
        IsLoading(true);
        infoList.Clear(); infoList.Title = AppResources.InfoReview;
        App.appSetting.DeviceInfoList.ForEach(info => {
           infoList.Add(new TextCell() { Text = info.Name, Detail = info.Value });
        });
        IsLoading(false);
    }


    //Load File List
    private async void BtnFilesButton_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true);
            infoList.Clear(); App.appSetting.DeviceFileLists.Clear(); infoList.Title = AppResources.FileList;
            App.appSetting.BlueTooth.Characteristics.ForEach(async characteristic => {

                var liadFileCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbListFiles").FirstOrDefault();
                if (liadFileCharacteristic != null) {
                    if (characteristic.UUid.ToString().ToLower() == liadFileCharacteristic.Uuid) {

                        bool again = true;
                        while (again) {
                            List<string> fileInfo = System.Text.Encoding.UTF8.GetString((await characteristic.Characteristic.ReadAsync()).data).Split(";").ToList();
                            if (fileInfo[0].Length > 0) {
                                App.appSetting.DeviceFileLists.Add(new DeviceFileList() { Name = fileInfo[0], Length = fileInfo[1] });
                                var fileLine = new TextCell() { Text = fileInfo[0] };
                                fileLine.Tapped += FileDownload_Tapped;
                                infoList.Add(fileLine);
                            } else { again = false; }
                        }
                    }
                }

            });
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false);
    }


    //Download Device File
    private async void FileDownload_Tapped(object sender, EventArgs e) {
        try {
            IsLoading(true);
            infoList.Clear(); App.appSetting.DeviceFileLists.Clear(); infoList.Title = AppResources.FileList;
            App.appSetting.BlueTooth.Characteristics.ForEach(async characteristic => {

                var liadFileCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbListFiles").FirstOrDefault();
                if (liadFileCharacteristic != null) {
                    if (characteristic.UUid.ToString().ToLower() == liadFileCharacteristic.Uuid) {

                        bool again = true;
                        while (again) {
                            List<string> fileInfo = System.Text.Encoding.UTF8.GetString((await characteristic.Characteristic.ReadAsync()).data).Split(";").ToList();
                            if (fileInfo[0].Length > 0) {
                                App.appSetting.DeviceFileLists.Add(new DeviceFileList() { Name = fileInfo[0], Length = fileInfo[1] });
                                var fileLine = new TextCell() { Text = fileInfo[0] };
                                fileLine.Tapped += FileDownload_Tapped;
                                infoList.Add(fileLine);
                            }
                            else { again = false; }
                        }
                    }

                }

            });
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false);
    }
}

