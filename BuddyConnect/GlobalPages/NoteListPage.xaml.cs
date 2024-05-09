using BuddyConnect.Resources.Languages;
using BuddyConnect.Controls;
using System.Linq;
using System.Globalization;
using BuddyConnect.DatabaseModel;
using BuddyConnect.Controllers;


namespace BuddyConnect;


public partial class NoteListPage : ContentPage, GlobalServices
{

    public NoteListPage() {

        InitializeComponent();
        _ = LoadStartUpData();
    }


    public async Task Dismiss() { await Navigation.PopModalAsync(); }


    public async Task<bool> LoadStartUpData() {
        noteTable.Clear();
        var table = new TableSection(AppResources.ResourceManager.GetString("YourNotes", new CultureInfo(App.appSetting.Language)));
        App.appSetting.Notes.ForEach(note => { table.Add(new TextCell() { Text = note.Timestamp.ToString(), Detail = note.Message }); });
        noteTable.Add(table);
        return true;
    }



    //private async void DeleteButton_Clicked(object sender, EventArgs e) {
    //    if (BindingContext is Note note) { if (File.Exists(note.Filename)) { File.Delete(note.Filename); } }
    //    await Shell.Current.GoToAsync("..");
    //}

    private async void AddNote_Clicked(object sender, EventArgs e) {
        string action = await DisplayPromptAsync(AppResources.AddNote, null, AppResources.Save, AppResources.Cancel, AppResources.WriteNoteHere, -1, null, "");
        if (action != null) {
            await NoteListController.InsertOrUpdateNoteList(new NoteList() { Message = action });
            App.appSetting.Notes = await NoteListController.GetNoteList();
            await LoadStartUpData();
        }
    }

    //private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
    //    if (e.CurrentSelection.Count != 0) {
    //        var note = (Note)e.CurrentSelection[0];
    //    }
    //}
}