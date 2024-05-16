using BuddyConnect.DatabaseModel;
using Plugin.BLE.Abstractions.Contracts;
using SQLite;


namespace BuddyConnect {


    /// <summary>
    /// Central Variable Definitions
    /// For Using EveryWhere
    /// </summary>
    public class AppSetting {

        //Control Part
        public IServiceProvider ServiceProvider { get; set; }
        public SQLiteAsyncConnection Database { get; set; }
        
        //Central BlueTooth
        public BlueTooth BlueTooth { get; set; }
        public List<CharDeviceInfoDefList> CharDeviceInfoDefLists { get; set; }
        public List<CharDeviceActionDefList> CharDeviceActionDefLists { get; set; }


        //Data PART
        //public string Language { get; set; }
        //public string Theme { get; set; }
        //public string TranslatedTheme { get; set; }
        //public string DeviceName { get; set; }

        public List<SettingList> Settings { get; set; } = new List<SettingList>();
        public List<NoteList> Notes { get; set; }
        public List<DeviceInfoList> DeviceInfoList { get; set; } = new List<DeviceInfoList> { };
        public List<DeviceFileList> DeviceFileLists { get; set; } = new List<DeviceFileList> { };
    }


    public class BlueTooth {
        public IBluetoothLE Bluetooth { get; set; }
        public IAdapter BtAdapter { get; set; }
        public List<IDevice> BtAvailableDevices { get; set; } = new List<IDevice>();
        public List<BtCharacteristics> Characteristics { get; set; } = new List<BtCharacteristics> { };
    }



    //Temp Definitions
    public class DeviceInfoList { 
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class DeviceFileList {
        public string Name { get; set; }
        public string Length { get; set; }
        public string Value { get; set; }
    }


    public class BtServices {
        public string? ServiceName { get; set; }
        public Guid? UUid { get; set; }
        public IService Service { get; set; }
        public bool IsPrimary { get; set; }
        public IReadOnlyList<ICharacteristic> Characteristics { get; set; }
    }

    public class BtCharacteristics  {

        public string? Name { get; set; }
        public string? UUid { get; set; }
        public ICharacteristic Characteristic { get; set; }
    }
}

