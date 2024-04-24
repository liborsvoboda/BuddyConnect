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
             new SettingList() { Key = "Language", Value ="cs-CZ"}
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
             new LanguageList() { Language = "cs-CZ", Name ="Česky"},
             new LanguageList() { Language = "en-US", Name ="English"},
             new LanguageList() { Language = "de-DE", Name ="Deutsch"}
        };
    }
}
