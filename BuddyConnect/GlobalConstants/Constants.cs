using SQLite;

namespace BuddyConnect {

    public static class Constants {
        public const string DatabaseFilename = "BuddyConnect.db3";

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.Current.AppDataDirectory , DatabaseFilename);
    }
}