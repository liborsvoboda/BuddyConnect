using SQLite;
using BuddyConnect.DatabaseModel;
using System.Diagnostics;


namespace BuddyConnect.Controllers {

    /// <summary>
    /// Table Controlers
    /// LanguageList 
    /// </summary>
    public static class LanguageListController {

        public static async Task<List<LanguageList>> GetLanguageList() {
            return await App.AppSetting.Database.Table<LanguageList>().ToListAsync();
        }


        public static async Task<LanguageList> GetLanguageListByLanguage(string language) {
            return await App.AppSetting.Database.Table<LanguageList>().Where(i => i.Language == language).FirstOrDefaultAsync();
        }


        //public static async Task<int> SaveLanguageList(LanguageList item) {
        //    if (item.Id != 0) {
        //        return await App.AppSetting.Database.UpdateAsync(item);
        //    }
        //    else { return await App.AppSetting.Database.InsertAsync(item); }
        //}


        public static async Task<int> SaveLanguageList(LanguageList item) {
            try {
                return await App.AppSetting.Database.InsertAsync(item);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        public static async Task<int> SaveLanguageListRange(List<LanguageList> item) {
            try { 
                return await App.AppSetting.Database.InsertAllAsync(item);
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