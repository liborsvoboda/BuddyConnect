using SQLite;
using BuddyConnect.DatabaseModel;
using System.Diagnostics;
using BuddyConnect.Functions;


namespace BuddyConnect.Controllers {

    /// <summary>
    /// Table Controlers
    /// LanguageList 
    /// </summary>
    public static class DetectedErrorListController {

        public static async Task<List<DetectedErrorList>> GetDetectedErrorList() {
            return await App.appSetting.Database.Table<DetectedErrorList>().ToListAsync();
        }


        public static async Task<DetectedErrorList> GetDetectedErrorListListById(int id) {
            return await App.appSetting.Database.Table<DetectedErrorList>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }


        public static async Task<int> InsertOrUpdateDetectedErrorList(DetectedErrorList item) {
            try {
                if (item.Id != 0) {
                    return await App.appSetting.Database.UpdateAsync(item);
                } else { return await App.appSetting.Database.InsertAsync(item); }
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }


        public static async Task<int> SaveDetectedErrorList(DetectedErrorList item) {
            try {
                return await App.appSetting.Database.InsertAsync(item);
            } catch (Exception ex) {  }
            return 0;
        }


        public static async Task<int> SaveDetectedErrorListRange(List<DetectedErrorList> item) {
            try { 
                return await App.appSetting.Database.InsertAllAsync(item);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }

        public static async Task<int> DeleteErrorListItemAsync(DetectedErrorList item) {
            try {
                return await App.appSetting.Database.DeleteAsync(item);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }

        public static async Task<int> DeleteAllErrorListAsync() {
            try {
                var data = await GetDetectedErrorList();
                return await App.appSetting.Database.DeleteAsync(data);
            } catch (Exception ex) {
                await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
                
            }
            return 0;
        }

    }
}