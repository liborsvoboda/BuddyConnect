using BuddyConect.Resources.Languages;
using BuddyConnect.Controls;
using System.Linq;


namespace BuddyConnect;


public partial class NoteListPage : ContentPage, IModalPage
{

    public NoteListPage() {

        InitializeComponent();
        _ = LoadStartUpData();

        string appDataPath = FileSystem.AppDataDirectory;
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

        LoadNote(Path.Combine(appDataPath, randomFileName));
    }


    public async Task Dismiss() { await Navigation.PopModalAsync(); }


    public async Task<bool> LoadStartUpData() {
        App.AppSetting.Notes = new AllNotes().Notes;
        notesCollection.ItemsSource = App.AppSetting.Notes;
        return true;
    }


    private void LoadNote(string fileName) {
        Note noteModel = new Note { Filename = fileName };

        if (File.Exists(fileName)) {
            noteModel.Date = File.GetCreationTime(fileName);
            noteModel.Text = File.ReadAllText(fileName);
        }
        BindingContext = noteModel;
    }


    //private async void DeleteButton_Clicked(object sender, EventArgs e) {
    //    if (BindingContext is Note note) { if (File.Exists(note.Filename)) { File.Delete(note.Filename); } }
    //    await Shell.Current.GoToAsync("..");
    //}

    private async void AddNote_Clicked(object sender, EventArgs e) {
        string action = await DisplayPromptAsync(AppResources.AddNote, null, AppResources.Save, AppResources.Cancel, AppResources.WriteNoteHere, -1, null, "");
        if (action != null) {
            if (BindingContext is Note note) { File.WriteAllText(note.Filename, action); }
            LoadStartUpData();
        }
    }

    private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (e.CurrentSelection.Count != 0) {
            var note = (Note)e.CurrentSelection[0];
        }
    }
}