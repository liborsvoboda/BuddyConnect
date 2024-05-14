using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;

namespace BuddyConnect
{
    public partial class ContactListPage : ContentPage, GlobalServices {


        public ContactListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }

        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }

        //Startup And OnRoute Change
        public async Task<bool> LoadStartUpData() {
            TranslatePageObjects();
            return true;
        }

        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {
            //btn_AddNote.Text = AppResources.AddNote;

        }


    }
}
