using SQLite;
using BuddyConnect.DatabaseModel;
using System.Diagnostics;
using BuddyConnect.Functions;


namespace BuddyConnect.Controllers {


    public static class CharDeviceActionDefListController {

        public static async Task<List<CharDeviceActionDefList>> GetCharDeviceActionDefList() {
            return await App.appSetting.Database.Table<CharDeviceActionDefList>().ToListAsync();
        }


        public static async Task<CharDeviceActionDefList> GetLanguageListByUuid(string uuid) {
            return await App.appSetting.Database.Table<CharDeviceActionDefList>().Where(i => i.Uuid == uuid).FirstOrDefaultAsync();
        }


        //public static async Task<int> SaveLanguageList(LanguageList item) {
        //    if (item.Id != 0) {
        //        return await App.appSetting.Database.UpdateAsync(item);
        //    }
        //    else { return await App.appSetting.Database.InsertAsync(item); }
        //}


        public static async Task<int> SaveCharDeviceActionDefList(CharDeviceActionDefList item) {
            try {
                return await App.appSetting.Database.InsertAsync(item);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }


        public static async Task<int> SaveCharDeviceActionDefListRange(List<CharDeviceActionDefList> item) {
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