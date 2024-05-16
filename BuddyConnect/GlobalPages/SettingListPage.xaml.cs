using BuddyConnect.Controllers;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;


namespace BuddyConnect
{
    public partial class SettingListPage : ContentPage, GlobalServices {
        

        public SettingListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
            

        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }


        public async Task<bool> LoadStartUpData() {
            txt_deviceName.Text = App.appSetting.Settings.Where(a => a.Key == "DeviceName").FirstOrDefault().Value;
            txt_webPage.Text = App.appSetting.Settings.Where(a => a.Key == "WebPage").FirstOrDefault().Value;
            txt_deviceSearchTimeOut.Text = App.appSetting.Settings.Where(a => a.Key == "DeviceSearchTimeOut").FirstOrDefault().Value;
            TranslatePageObjects();
            return true;
        }


        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {
            lbl_deviceName.Text = AppResources.SearchedDeviceName;
            txt_deviceName.Placeholder = AppResources.SearchDeviceName;
            lbl_deviceSearchTimeOut.Text = AppResources.DeviceSearchTimeOut;
            txt_deviceSearchTimeOut.Placeholder = AppResources.SearchTimeInSec;
            lbl_webPage.Text = AppResources.WebViewPage;
            txt_webPage.Placeholder = AppResources.MenuWebViewPage;
            btn_save.Text = AppResources.Save;
        }


        private async void BtnSave_Clicked(object sender, EventArgs e) {
            try {
                aiLoading.IsRunning = true;
                await SettingListController.InsertOrUpdateSettingListAsync(new SettingList() { Key = "DeviceName", Value = txt_deviceName.Text });
                await SettingListController.InsertOrUpdateSettingListAsync(new SettingList() { Key = "WebPage", Value = txt_webPage.Text });
                await SettingListController.InsertOrUpdateSettingListAsync(new SettingList() { Key = "DeviceSearchTimeOut", Value = txt_deviceSearchTimeOut.Text });
                
                App.appSetting.Settings = await SettingListController.GetSettingList();
                await LoadStartUpData();

            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
            }
            aiLoading.IsRunning = false;
        }
    }
}
