using SQLite;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Resources.Languages;
using System.Diagnostics;

namespace BuddyConnect.Controllers {

    /// <summary>
    /// Table Controlers
    /// SettingList 
    /// </summary>
    public class SettingListController {

        public static async Task<List<SettingList>> GetSettingList() {
            return await App.AppSetting.Database.Table<SettingList>().ToListAsync();
        }


        public static async Task<SettingList> GetSettingListByKey(string key) {
            return await App.AppSetting.Database.Table<SettingList>().Where(i => i.Key == key).FirstOrDefaultAsync();
        }


        //public static async Task<int> SaveSettingList(SettingList item) {
        //    if (item.Id != 0) {
        //        return await App.AppSetting.Database.UpdateAsync(item);
        //    }
        //    else { return await App.AppSetting.Database.InsertAsync(item); }
        //}

        public static async Task<int> SaveSettingList(SettingList item) {
            try { 
                return await App.AppSetting.Database.InsertAsync(item);
            } 
            catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        public static async Task<int> SaveSettingListRange(List<SettingList> item) {
            try { 
                return await App.AppSetting.Database.InsertAllAsync(item);
            } 
            catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        //Get/Set Configuration Changing


        public static async Task<SettingList> LoadSettingListThemeStartup() {
            return await App.AppSetting.Database.Table<SettingList>().Where(i => i.Key == "Theme").FirstOrDefaultAsync();
        }

        public static async Task<SettingList> LoadSettingListLanguageStartup() {
            return await App.AppSetting.Database.Table<SettingList>().Where(i => i.Key == "Language").FirstOrDefaultAsync();
        }


        public static async Task<int> SetSelectedLanguage(string language) {
            try { 
                SettingList selectedLanguage = new() { Key = "Language", Value = language };
                App.AppSetting.Language = language;
                return await App.AppSetting.Database.UpdateAsync(selectedLanguage);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        public static async Task<int> SetSelectedTheme(string theme) {
            try { 
                SettingList selectedTheme = new SettingList() { Key = "Theme", Value = theme };
                App.AppSetting.Theme = theme;
                App.AppSetting.TranslatedTheme = AppResources.ResourceManager.GetString(theme);
                return await App.AppSetting.Database.UpdateAsync(selectedTheme);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }

        //public async Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    await Init();
        //    return await Database.DeleteAsync(item);
        //}


    }
}