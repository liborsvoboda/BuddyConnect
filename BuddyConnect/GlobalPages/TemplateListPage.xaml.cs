using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class TemplateListPage : ContentPage, GlobalServices {
        public TemplateListPage() {
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

        /*
        //Change Theme
        private async void OnThemeToolbarItemClicked(object sender, EventArgs e) {
            ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme(true);
        }
        */


    }
}
