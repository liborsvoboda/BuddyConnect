using BuddyConnect.Controls;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using SQLite;
using System.Collections.ObjectModel;
using BuddyConnect.DatabaseModel;

namespace BuddyConnect {


    /// <summary>
    /// Central Variable Definitions
    /// For Using EveryWhere
    /// </summary>
    public class AppSetting {

        //Control Part
        public IServiceProvider ServiceProvider { get; set; }
        public SQLiteAsyncConnection Database { get; set; }
        public BlueTooth BlueTooth { get; set; }


        //Data PART
        public string Language { get; set; }
        public string Theme { get; set; }
        public string TranslatedTheme { get; set; }
        public string DeviceName { get; set; }

        public List<DeviceList> Devices { get; set; }
        public List<NoteList> Notes { get; set; }
    }


    public class BlueTooth {
        public IBluetoothLE Bluetooth { get; set; }
        public IAdapter BtAdapter { get; set; }
        public List<IDevice> BtAvailableDevices { get; set; } = new List<IDevice>();
    }
}

