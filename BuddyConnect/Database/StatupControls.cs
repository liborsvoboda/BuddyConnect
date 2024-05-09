using SQLite;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Resources.Languages;
using System.Diagnostics;
using BuddyConnect.Functions;

namespace BuddyConnect {

    public class StatupControls {

        //Database Init
        public static async Task StartupInit() {
            try {
                if (App.appSetting.Database is not null)
                    return;

                App.appSetting.Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

                //Create And Set Default In Database
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(LanguageList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<LanguageList>();
                    if ((await Controllers.LanguageListController.GetLanguageList()).Count == 0) {
                        await Controllers.LanguageListController.SaveLanguageListRange(DefaultLanguageList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(SettingList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<SettingList>();
                    if ((await Controllers.SettingListController.GetSettingList()).Count == 0) {
                        await Controllers.SettingListController.SaveSettingListRange(DefaultSettingList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(NoteList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<NoteList>();
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(DeviceList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<DeviceList>();
                }

                //Load App Settings
                await LoadStartupSettings();
            } 
            catch (Exception ex) { Debug.WriteLine(ex); }
        }


        /// <summary>
        /// Set App class to Settings Values
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> LoadStartupSettings() {

            //Load Variables
            App.appSetting.Theme = (await Controllers.SettingListController.GetSettingListByKey("Theme")).Value;
            App.appSetting.TranslatedTheme = AppResources.ResourceManager.GetString(App.appSetting.Theme);
            App.appSetting.Language = (await Controllers.SettingListController.GetSettingListByKey("Language")).Value;
            App.appSetting.DeviceName = (await Controllers.SettingListController.GetSettingListByKey("DeviceName")).Value;

            App.appSetting.Notes = (await Controllers.NoteListController.GetNoteList());
            App.appSetting.Devices = (await Controllers.DeviceListController.GetDeviceList());

            //Configure Theme
            await SystemFunctions.ChangeorLoadTheme();

            //Set Language 
            await SystemFunctions.ChangeorLoadLanguage();

            return true;
        }



    }
}