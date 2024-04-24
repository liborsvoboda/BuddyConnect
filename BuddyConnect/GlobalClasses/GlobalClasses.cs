using BuddyConnect.Controls;
using SQLite;
using System.Collections.ObjectModel;

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
        public ObservableCollection<Note> Notes { get; set; }
    }

    public class Note {
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}

