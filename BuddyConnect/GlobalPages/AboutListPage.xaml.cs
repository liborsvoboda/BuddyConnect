using BuddyConnect.Resources.Languages;
using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class AboutListPage : ContentPage, GlobalServices {


        public AboutListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }

        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }

        public async Task<bool> LoadStartUpData() {
            TranslatePageObjects();
            return true;
        }


        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {
            lbl_appName.Text = AppResources.AppName;
            lbl_version.Text = AppResources.Version;
            lbl_appDesc.Text = AppResources.AppDescription;
            btn_checkUpdate.Text = AppResources.CheckUpdate;
            btn_openWebsite.Text = AppResources.OpenWebsite;
        }


        private async void LearnMore_Clicked(object sender, EventArgs e) {
            await Launcher.Default.OpenAsync(AppResources.WebsiteUrl);
        }

        private void CheckUpdate_Clicked(object sender, EventArgs e) {

        }
    }
}
