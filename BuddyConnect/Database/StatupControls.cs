using SQLite;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Resources.Languages;
using System.Diagnostics;
using BuddyConnect.Functions;
using BuddyConnect.Controllers;

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
                    if ((await LanguageListController.GetLanguageList()).Count == 0) {
                        await LanguageListController.SaveLanguageListRange(DefaultLanguageList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(SettingList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<SettingList>();
                    if ((await SettingListController.GetSettingList()).Count == 0) {
                        await SettingListController.SaveSettingListRange(DefaultSettingList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(NoteList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<NoteList>();
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(DeviceList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<DeviceList>();
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(CharDeviceInfoDefList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<CharDeviceInfoDefList>();
                    if ((await CharDeviceInfoDefListController.GetCharDeviceInfoDefList()).Count == 0) {
                        await CharDeviceInfoDefListController.SaveCharDeviceInfoDefListRange(DefaultCharDeviceInfoDefList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(CharDeviceActionDefList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<CharDeviceActionDefList>();
                    if ((await CharDeviceActionDefListController.GetCharDeviceActionDefList()).Count == 0) {
                        await CharDeviceActionDefListController.SaveCharDeviceActionDefListRange(DefaultCharDeviceActionDefList.DefaultItems);
                    }
                }
                if ((await App.appSetting.Database.GetTableInfoAsync(nameof(DetectedErrorList))).Count == 0) {
                    await App.appSetting.Database.CreateTableAsync<DetectedErrorList>();
                }
                

                //Load App Settings
                await LoadStartupSettings();
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }

        }


        /// <summary>
        /// Set App class to Settings Values
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> LoadStartupSettings() {
            try {
                //Load Variables
                App.appSetting.Theme = (await SettingListController.GetSettingListByKey("Theme")).Value;
                App.appSetting.TranslatedTheme = AppResources.ResourceManager.GetString(App.appSetting.Theme);
                App.appSetting.Language = (await SettingListController.GetSettingListByKey("Language")).Value;
                App.appSetting.DeviceName = (await SettingListController.GetSettingListByKey("DeviceName")).Value;

                App.appSetting.CharDeviceInfoDefLists = (await CharDeviceInfoDefListController.GetCharDeviceInfoDefList());
                App.appSetting.CharDeviceActionDefLists = (await CharDeviceActionDefListController.GetCharDeviceActionDefList());
                App.appSetting.Notes = (await NoteListController.GetNoteList());
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }

            //Configure Theme
            await SystemFunctions.ChangeorLoadTheme();

            //Set Language 
            await SystemFunctions.ChangeorLoadLanguage();

            return true;
        }



    }
}