using BuddyConnect.Resources.Languages;
using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class AboutListPage : ContentPage, IModalPage {


        public AboutListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }

        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }

        public async Task<bool> LoadStartUpData() {
            //ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme();
            return true;
        }


        private async void LearnMore_Clicked(object sender, EventArgs e) {
            await Launcher.Default.OpenAsync(AppResources.WebsiteUrl);
        }

        /*
        //Change Theme
        private async void OnThemeToolbarItemClicked(object sender, EventArgs e) {
            ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme(true);
        }
        */


    }
}
