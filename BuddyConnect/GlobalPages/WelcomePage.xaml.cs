using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;
using System.Diagnostics;

namespace BuddyConnect
{
    public partial class WelcomePage : ContentPage, GlobalServices {


        public WelcomePage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }


        //Solve Load Startup Data and Remove Welcome Page
        public async Task<bool> LoadStartUpData() {
            TranslatePageObjects();

            await Heart.ScaleTo(1.3, 1000); await Heart.ScaleTo(1, 1000);
            await Heart.ScaleTo(1.3, 1000); await Heart.ScaleTo(1, 1000);

            ((Shell)App.Current.MainPage).Items.RemoveAt(0);
            await this.Navigation.PopToRootAsync();
            return true;
        }

        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {
            lbl_loading.Text = AppResources.Loading;

        }
    }
}
