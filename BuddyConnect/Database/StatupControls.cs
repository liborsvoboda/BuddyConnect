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
                if (App.AppSetting.Database is not null)
                    return;

                App.AppSetting.Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

                //Create And Set Default In Database
                if ((await App.AppSetting.Database.GetTableInfoAsync(nameof(LanguageList))).Count == 0) {
                    await App.AppSetting.Database.CreateTableAsync<LanguageList>();
                    if ((await Controllers.LanguageListController.GetLanguageList()).Count == 0) {
                        await Controllers.LanguageListController.SaveLanguageListRange(DefaultLanguageList.DefaultItems.ToList());
                    }
                }
                if ((await App.AppSetting.Database.GetTableInfoAsync(nameof(SettingList))).Count == 0) {
                    await App.AppSetting.Database.CreateTableAsync<SettingList>();
                    if ((await Controllers.SettingListController.GetSettingList()).Count == 0) {
                        await Controllers.SettingListController.SaveSettingListRange(DefaultSettingList.DefaultItems);
                    }
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
            App.AppSetting.Theme = (await Controllers.SettingListController.GetSettingListByKey("Theme")).Value;
            App.AppSetting.TranslatedTheme = AppResources.ResourceManager.GetString(App.AppSetting.Theme);
            App.AppSetting.Language = (await Controllers.SettingListController.GetSettingListByKey("Language")).Value;

            //Configure Theme
            await SystemFunctions.ChangeorLoadTheme();

            //Set Language 
            await SystemFunctions.ChangeorLoadLanguage();

            return true;
        }



    }
}