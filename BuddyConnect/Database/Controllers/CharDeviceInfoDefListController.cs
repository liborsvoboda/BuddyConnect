using SQLite;
using BuddyConnect.DatabaseModel;
using System.Diagnostics;
using BuddyConnect.Functions;


namespace BuddyConnect.Controllers {

    /// <summary>
    /// Table Controlers
    /// LanguageList 
    /// </summary>
    public static class CharDeviceInfoDefListController {

        public static async Task<List<CharDeviceInfoDefList>> GetCharDeviceInfoDefList() {
            return await App.appSetting.Database.Table<CharDeviceInfoDefList>().ToListAsync();
        }


        public static async Task<CharDeviceInfoDefList> GetCharDeviceInfoDefListByName(string name) {
            return await App.appSetting.Database.Table<CharDeviceInfoDefList>().Where(i => i.CharName == name).FirstOrDefaultAsync();
        }


        //public static async Task<int> SaveLanguageList(LanguageList item) {
        //    if (item.Id != 0) {
        //        return await App.appSetting.Database.UpdateAsync(item);
        //    }
        //    else { return await App.appSetting.Database.InsertAsync(item); }
        //}


        public static async Task<int> SaveCharDeviceInfoDefList(CharDeviceInfoDefList item) {
            try {
                return await App.appSetting.Database.InsertAsync(item);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }


        public static async Task<int> SaveCharDeviceInfoDefListRange(List<CharDeviceInfoDefList> item) {
            try { 
                return await App.appSetting.Database.InsertAllAsync(item);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }

        //public async Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    await Init();
        //    return await Database.DeleteAsync(item);
        //}


    }
}