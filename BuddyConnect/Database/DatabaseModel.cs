using SQLite;


namespace BuddyConnect.DatabaseModel {

    /// <summary>
    /// Application Settings Table
    /// Primary  Key must Exist 
    /// </summary>
    public class SettingList {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }
        [PrimaryKey]
        [Indexed(Name = "UI_KeyId", Unique = true)]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class DefaultSettingList {
        public static List<SettingList> DefaultItems = new List<SettingList>() {
             new SettingList() { Key = "Theme", Value ="Light"},
             new SettingList() { Key = "Language", Value ="cs"},
             new SettingList() { Key = "DeviceName", Value ="DVBdiver"},
             new SettingList() { Key = "WebPage", Value ="https://kliknetezde.cz/"}
        };
    }

    public class LanguageList {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }
        [PrimaryKey]
        [Indexed(Name = "UI_LanguageId", Unique = true)]
        public string Language { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class DefaultLanguageList {
        public static List<LanguageList> DefaultItems = new List<LanguageList>() {
             new LanguageList() { Language = "cs", Name ="Česky"},
             new LanguageList() { Language = "en", Name ="English"},
             new LanguageList() { Language = "de", Name ="Deutsch"}
        };
    }

    public class NoteList {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }


    public class DeviceList {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "UI_NameId", Unique = true)]
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }


    public class CharDeviceInfoDefList {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "UI_CharNameId", Unique = true)]
        public string CharName { get; set; }
        public string Uuid { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }


    public class DefaultCharDeviceInfoDefList {
        public static List<CharDeviceInfoDefList> DefaultItems = new List<CharDeviceInfoDefList>() {
             new CharDeviceInfoDefList() { CharName = "Device Name" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Model Number String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Manufacturer Name String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Serial Number String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Firmware Revision String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Hardware Revision String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Software Revision String" ,Uuid = null},
             new CharDeviceInfoDefList() { CharName = "Dvb Serial Number" ,Uuid = "dbd00003-ff30-40a5-9ceb-a17358d31999"},
             new CharDeviceInfoDefList() { CharName = "Dvb Short Name" ,Uuid = "dbd00002-ff30-40a5-9ceb-a17358d31999"}
        };
    }


    public class CharDeviceActionDefList {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "UI_NameId", Unique = true)]
        public string Name { get; set; }
        public string Uuid { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }


    public class DefaultCharDeviceActionDefList {
        public static List<CharDeviceActionDefList> DefaultItems = new List<CharDeviceActionDefList>() {
             //new CharDeviceActionDefList() { Name = "Dvb Serial Number", Uuid = "dbd00003-ff30-40a5-9ceb-a17358d31999" },
             new CharDeviceActionDefList() { Name = "DvbService", Uuid = "dbd00001-ff30-40a5-9ceb-a17358d31999" },
             new CharDeviceActionDefList() { Name = "DvbListFiles", Uuid = "dbd00010-ff30-40a5-9ceb-a17358d31999" },
             //new CharDeviceActionDefList() { Name = "Short Name", Uuid = "dbd00002-ff30-40a5-9ceb-a17358d31999" },
             new CharDeviceActionDefList() { Name = "DvbWriteToDevice", Uuid = "dbd00011-ff30-40a5-9ceb-a17358d31999" },
             new CharDeviceActionDefList() { Name = "DvbReadFromDevice", Uuid = "dbd00012-ff30-40a5-9ceb-a17358d31999" },
             new CharDeviceActionDefList() { Name = "DvbFormatStorage", Uuid = "dbd00013-ff30-40a5-9ceb-a17358d31999" }
        };
    }



    public class DetectedErrorList {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "UI_MessageId", Unique = true)]
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
