using BuddyConnect.Resources.Languages;
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
        var table = new TableSection(AppResources.YourNotes);
        App.appSetting.Notes.OrderByDescending(a => a.Timestamp).ToList().ForEach(note => {
            var noteItem = new TextCell() { Text = note.Id.ToString() +": " + note.Timestamp.ToString(), Detail = note.Message  };
            noteItem.SetDynamicResource(TextCell.TextColorProperty, "PrimaryTextColor");
            noteItem.SetDynamicResource(TextCell.DetailColorProperty, "SecondaryTextColor");
            noteItem.Tapped += NoteItemDelete_Tapped;
            table.Add(noteItem);
        });
        noteTable.Add(table);
        TranslatePageObjects();
        return true;
    }


    //List Of All Translated Object For Reload By LoadStartUpData()
    private void TranslatePageObjects() {
        btn_AddNote.Text = AppResources.AddNote;

    }


    //Delete Note
    private async void NoteItemDelete_Tapped(object sender, EventArgs e) {
            string action = await DisplayActionSheet(AppResources.DeleteNoteQuestion, AppResources.Cancel, null, AppResources.Delete + " Id:"+ ((TextCell)sender).Text.Split(":")[0] );
        if (action == AppResources.Delete + " Id:" + ((TextCell)sender).Text.Split(":")[0]) {
            await NoteListController.DeleteNoteItemAsync(new NoteList() { Id = int.Parse(((TextCell)sender).Text.Split(":")[0]) });
            App.appSetting.Notes = await NoteListController.GetNoteList();
            await LoadStartUpData();
        }
    }

    //Insert Note
    private async void AddNote_Clicked(object sender, EventArgs e) {
        string action = await DisplayPromptAsync(AppResources.AddNote, null, AppResources.Save, AppResources.Cancel, AppResources.WriteNoteHere, -1, null, "");
        if (action != null) {
            await NoteListController.InsertOrUpdateNoteList(new NoteList() { Message = action });
            App.appSetting.Notes = await NoteListController.GetNoteList();
            await LoadStartUpData();
        }
    }

}