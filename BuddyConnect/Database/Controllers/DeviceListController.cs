using SQLite;
using BuddyConnect.DatabaseModel;
using System.Diagnostics;


namespace BuddyConnect.Controllers {

    /// <summary>
    /// Table Controlers
    /// LanguageList 
    /// </summary>
    public static class DeviceListController {

        public static async Task<List<DeviceList>> GetDeviceList() {
            return await App.appSetting.Database.Table<DeviceList>().ToListAsync();
        }


        public static async Task<DeviceList> GetDeviceListListByName(string name) {
            return await App.appSetting.Database.Table<DeviceList>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }


        public static async Task<int> InsertOrUpdateDeviceList(DeviceList item) {
            try {
                if (item.Id != 0) {
                    return await App.appSetting.Database.UpdateAsync(item);
                } else { return await App.appSetting.Database.InsertAsync(item); }
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        public static async Task<int> SaveDeviceList(DeviceList item) {
            try {
                return await App.appSetting.Database.InsertAsync(item);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


        public static async Task<int> SaveDeviceListRange(List<DeviceList> item) {
            try { 
                return await App.appSetting.Database.InsertAllAsync(item);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }

        public static async Task<int> DeleteDeviceItemAsync(DeviceList item) {
            try {
                return await App.appSetting.Database.DeleteAsync(item);
            } catch (Exception ex) { Debug.WriteLine(ex); }
            return 0;
        }


    }
}