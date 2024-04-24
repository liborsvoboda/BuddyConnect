using SQLite;

namespace BuddyConnect {


    /// <summary>
    /// Central Variable Definitions
    /// For Using EveryWhere
    /// </summary>
    public class AppSetting {

        public SQLiteAsyncConnection Database { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
        public string TranslatedTheme { get; set; }

    }
}

