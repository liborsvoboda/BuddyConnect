using BuddyConnect.Controllers;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;
using CommunityToolkit.Maui.Storage;
using Plugin.BLE.Abstractions.Contracts;
using System.Text;



namespace BuddyConnect;

public partial class DeviceManagementPage : ContentPage, GlobalServices {


    public BlueTooth test = App.appSetting.BlueTooth;

    public DeviceManagementPage() {

        InitializeComponent();
        _ = CheckBtPermissions();

        ConfigureBLE();
        _ = LoadStartUpData();
    }



    private async void BtAdapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
        // App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
        await LoadStartUpData();
    }

    //Detect Devices By Name
    private void BtAdapter_DeviceAdvertised(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) {
        if (e.Device.Name != null && e.Device.Name.ToLower().Contains(App.appSetting.Settings.Where(a => a.Key == "DeviceName").FirstOrDefault().Value.ToLower())
            && (App.appSetting.BlueTooth.BtAvailableDevices.Count == 0 || (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0 && !App.appSetting.BlueTooth.BtAvailableDevices.Contains(e.Device)))) {
            App.appSetting.BlueTooth.BtAvailableDevices.Add(e.Device);
        }
    }

    //ScanTimeout Action
    private async void BtAdapter_ScanTimeoutElapsed(object sender, EventArgs e) => await LoadStartUpData();
    //BT State Changed Action
    private async void Bluetooth_StateChanged(object sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e) => await LoadStartUpData();


    //Set BT Device Status
    public async Task<bool> LoadStartUpData() {

        try {
            if (App.appSetting.BlueTooth.BtAdapter.ConnectedDevices.Count > 0) {
                lbl_status.Text = AppResources.Connected;
            }
            else if (!App.appSetting.BlueTooth.Bluetooth.IsAvailable) {
                lbl_status.Text = AppResources.NotAvailable;
                bt_scanButton.IsVisible = false;
                deviceList.Clear(); infoList.Clear();
                bt_infoButton.IsVisible = bt_filesButton.IsVisible = bt_formatButton.IsVisible = false;
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "off") {
                lbl_status.Text = AppResources.Off;
                bt_scanButton.IsVisible = false;
                deviceList.Clear(); infoList.Clear();
                bt_infoButton.IsVisible = bt_filesButton.IsVisible = bt_formatButton.IsVisible = false;
            }
            else if (App.appSetting.BlueTooth.Bluetooth.IsAvailable && App.appSetting.BlueTooth.Bluetooth.State.ToString().ToLower() == "on") {
                lbl_status.Text = AppResources.On;
                bt_scanButton.IsVisible = true;
            }
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }


        TranslatePageObjects();
        return true;
    }

    //List Of All Translated Object For Reload By LoadStartUpData()
    private void TranslatePageObjects() {
        lbl_btLabel.Text = AppResources.BtStatus;
        bt_scanButton.Text = AppResources.Search;
        deviceList.Title = AppResources.AvailableDevices;
        infoList.Title = AppResources.InfoReview;
        bt_infoButton.Text = AppResources.ShowDeviceInfo;
        bt_filesButton.Text = AppResources.ShowListFiles;
        bt_formatButton.Text = AppResources.CleanDevice;
    }
    public async Task Dismiss() { await Navigation.PopModalAsync(); }


    //Run BT Scanning 
    private async void BtScanButton_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true); ClearListBTdata();
            await ForgotBTDevices();//Forgot All 

            await App.appSetting.BlueTooth.BtAdapter.StartScanningForDevicesAsync();
            if (App.appSetting.BlueTooth.BtAvailableDevices.Count > 0) {

                App.appSetting.BlueTooth.BtAvailableDevices.ForEach(availableDevice => {
                    var textCell = new TextCell() { Text = availableDevice.Name }; textCell.Tapped += SelectDevice_Clicked; deviceList.Add(textCell);
                });
            }
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false);
        await LoadStartUpData();
    }


    //Load Data From Founded Device
    private async void SelectDevice_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true); ClearListBTdata();
            await DisconnectDevice();//Disconnect Device

            //Connect To BT Device
            var adapter = App.appSetting.BlueTooth.BtAdapter;
            await adapter.ConnectToDeviceAsync(App.appSetting.BlueTooth?.BtAvailableDevices?.First(a => a.Name.ToString() == ((TextCell)sender).Text), default);
            var device = App.appSetting.BlueTooth.BtAdapter.ConnectedDevices?.ToList()[0];

            IReadOnlyList<IService> services = await device.GetServicesAsync(default);
            foreach (var service in services) {
                //Load CHaracteristics And Info Values
                IReadOnlyList<ICharacteristic> characteristics = await service.GetCharacteristicsAsync();
                foreach (ICharacteristic characteristic in characteristics) {
                    App.appSetting.BlueTooth.Characteristics.Add(new BtCharacteristics() { Name = characteristic?.Name, UUid = characteristic.Id.ToString(), Characteristic = characteristic });

                    if (App.appSetting.CharDeviceInfoDefLists.Where(a => a.CharName == characteristic?.Name && a.Uuid == null).Count() > 0) {
                        //Insert Standard BT Info by Name
                        string value = Encoding.UTF8.GetString((await characteristic.ReadAsync()).data);
                        App.appSetting.DeviceInfoList.Add(new DeviceInfoList() { Name = characteristic?.Name, Value = value });
                        infoList.Add(new TextCell() { Text = characteristic?.Name, Detail = value });

                    } else if (App.appSetting.CharDeviceInfoDefLists.Where(a => a.Uuid != null && a.Uuid.ToLower() == characteristic?.Uuid.ToString().ToLower()).Count() > 0) {
                        //Insert DVB info by Uuid
                        CharDeviceInfoDefList customInfo = App.appSetting.CharDeviceInfoDefLists.Where(a => a.Uuid != null && a.Uuid.ToLower() == characteristic?.Uuid.ToString().ToLower()).FirstOrDefault();

                        string value = Encoding.UTF8.GetString((await characteristic.ReadAsync()).data);
                        App.appSetting.DeviceInfoList.Add(new DeviceInfoList() { Name = customInfo.CharName, Value = value });
                        infoList.Add(new TextCell() { Text = customInfo.CharName, Detail = value });
                    }
                }
            }

            //Show Action Buttons
            bt_infoButton.IsVisible = bt_filesButton.IsVisible = bt_formatButton.IsVisible = true;
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
            infoList.Clear(); infoList.Title = AppResources.FileList;


            if (App.appSetting.DeviceFileLists.Count > 0) {
                //Show  Loaded FileList
                App.appSetting.DeviceFileLists.ForEach(file => {
                    var fileLine = new TextCell() { Text = file.Name, Detail = AppResources.Size + ": " + file.Length };
                    fileLine.Tapped += FileDownload_Clicked;
                    infoList.Add(fileLine);
                });
            } else {
                //Load FileList
                var loadFileListCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbListFiles").LastOrDefault();
                var fileListCharacteristic = App.appSetting.BlueTooth.Characteristics.Where(a => a.Characteristic.Uuid.ToString().ToLower() == loadFileListCharacteristic.Uuid.ToLower()).LastOrDefault();

                if (fileListCharacteristic != null) {
                    bool again = true;
                    while (again) {
                        List<string> fileInfo = Encoding.UTF8.GetString((await fileListCharacteristic.Characteristic.ReadAsync()).data).Split(";").ToList();
                        if (fileInfo[0].Length > 0) { // && int.Parse(fileInfo[1]) > 0
                            App.appSetting.DeviceFileLists.Add(new DeviceFileList() { Name = fileInfo[0], Length = fileInfo[1] });
                            var fileLine = new TextCell() { Text = fileInfo[0], Detail = AppResources.Size + ": " + fileInfo[1] };
                            fileLine.Tapped += FileDownload_Clicked;
                            infoList.Add(fileLine);
                        } else { again = false; }
                    }
                }
            }
        } catch (Exception ex) {
            await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
        }
        IsLoading(false);
    }


    //TODO Download Device File
    private async void FileDownload_Clicked(object sender, EventArgs e) {
        try {
            IsLoading(true);
            
            var loadWriteCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbWriteToDevice").LastOrDefault();
            var loadReadCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbReadFromDevice").LastOrDefault();

            var writeCharacteristic = App.appSetting.BlueTooth.Characteristics.Where(a => a.Characteristic.Uuid.ToString().ToLower() == loadWriteCharacteristic.Uuid.ToLower()).LastOrDefault();
            var readCharacteristic = App.appSetting.BlueTooth.Characteristics.Where(a => a.Characteristic.Uuid.ToString().ToLower() == loadReadCharacteristic.Uuid.ToLower()).LastOrDefault();

            await writeCharacteristic.Characteristic?.WriteAsync(Encoding.UTF8.GetBytes($"{((TextCell)sender).Text};{((TextCell)sender).Detail.Split(": ")[1]};"), default);
            byte[] fileData = (await readCharacteristic.Characteristic.ReadAsync(default)).data;
            if (fileData.Length > 0) {

                using var stream = new MemoryStream(fileData);
                var fileSaveResult = await FileSaver.Default.SaveAsync(((TextCell)sender).Text, stream, default);

                //if (fileSaveResult.IsSuccessful) { await Toast.Make($"File is saved: {fileSaveResult.FilePath}").Show(default);
                //} else { await Toast.Make($"File is not saved, {fileSaveResult.Exception.Message}").Show(default); }

                //using (FileStream stream = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, ((TextCell)sender).Text), FileMode.Create)) {
                //    await stream.WriteAsync(fileData.data);
                //}
            }
        }
        catch (Exception ex) { await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) }); }
        IsLoading(false);
    }


    //Format Device Data 
    private async void BtnFormatButton_Clicked(object sender, EventArgs e) {
        bool action = await DisplayAlert(AppResources.CleanDevice, AppResources.CleanDeviceQuestion, AppResources.Yes, AppResources.Cancel);
        if (action) { await FormatDevice(); ClearListBTdata(); }
        IsLoading(false);
    }





    ///FUNCTION PART 
     // Set up scanner
    private void ConfigureBLE() {
        App.appSetting.BlueTooth.BtAdapter.ScanMode = ScanMode.LowLatency; App.appSetting.BlueTooth.BtAdapter.ScanTimeout = 10000; // ms
        App.appSetting.BlueTooth.Bluetooth.StateChanged += Bluetooth_StateChanged; App.appSetting.BlueTooth.BtAdapter.ScanTimeoutElapsed += BtAdapter_ScanTimeoutElapsed;
        App.appSetting.BlueTooth.BtAdapter.DeviceAdvertised += BtAdapter_DeviceAdvertised; App.appSetting.BlueTooth.BtAdapter.DeviceDiscovered += BtAdapter_DeviceDiscovered; ;
    }


    //Check BT Permissions
    private async Task<bool> CheckBtPermissions() {
        var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
        if (permissionResult != PermissionStatus.Granted) { permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>(); }
        if (permissionResult != PermissionStatus.Granted) { AppInfo.ShowSettingsUI(); return false; }
        return true;
    }


    //Disconnect and Clear Datasets
    private async Task<bool> ForgotBTDevices() {
        await DisconnectDevice();
        App.appSetting.BlueTooth.BtAvailableDevices.Clear(); App.appSetting.BlueTooth.Characteristics.Clear();
        deviceList.Clear(); ClearListBTdata();
        return true;
    }


    //Disconnect Device
    private async Task<bool> DisconnectDevice() {
        foreach (var device in App.appSetting.BlueTooth.BtAdapter.ConnectedDevices) { await App.appSetting.BlueTooth.BtAdapter.DisconnectDeviceAsync(device); }
        return true;
    }
    //Clear Readed BTdata 
    private void ClearListBTdata() {
        App.appSetting.BlueTooth.Characteristics.Clear();
        infoList.Clear();
        App.appSetting.DeviceInfoList.Clear(); App.appSetting.DeviceFileLists.Clear();
        infoList.Title = AppResources.InfoReview;
        bt_infoButton.IsVisible = bt_filesButton.IsVisible = bt_formatButton.IsVisible = false;
    }


    //Set Indicator IsLoading
    private void IsLoading(bool isLoading) { //((Button)button).BackgroundColor = Color.Parse("Red");
        if (isLoading) {  PageContent.Children.Where(a => a.GetType().Name == "Button").ToList().ForEach(button => { ((Button)button).IsEnabled = false; });
        } else { PageContent.Children.Where(a => a.GetType().Name == "Button").ToList().ForEach(button => { ((Button)button).IsEnabled = true; }); }
        aiLoading.IsRunning = isLoading;
    }


    //Device Format Function Do Reset Device
    private async Task<bool> FormatDevice() {
        try {
            IsLoading(true);
            infoList.Clear(); App.appSetting.DeviceFileLists.Clear(); infoList.Title = AppResources.FileList;
            var loadFormatCharacteristic = App.appSetting.CharDeviceActionDefLists.Where(a => a.Name == "DvbFormatStorage").LastOrDefault();
            var formatCharacteristic = App.appSetting.BlueTooth.Characteristics.Where(a => a.Characteristic.Uuid.ToString().ToLower() == loadFormatCharacteristic.Uuid.ToLower()).LastOrDefault();
            await formatCharacteristic.Characteristic.ReadAsync();
        } catch { }
        IsLoading(false);
        return true;
    }


}

