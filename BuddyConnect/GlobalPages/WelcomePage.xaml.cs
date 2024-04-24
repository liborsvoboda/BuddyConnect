using BuddyConnect.Functions;
using System.Diagnostics;

namespace BuddyConnect
{
    public partial class WelcomePage : ContentPage, IModalPage {


        public WelcomePage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }


        //Solve Load Startup Data and Remove Welcome Page
        public async Task<bool> LoadStartUpData() {
            await StatupControls.StartupInit();

            await Heart.ScaleTo(1.3, 1000); await Heart.ScaleTo(1, 1000);
            await Heart.ScaleTo(1.3, 1000); await Heart.ScaleTo(1, 1000);

            ((Shell)App.Current.MainPage).Items.RemoveAt(0);
            await this.Navigation.PopToRootAsync();
            return true;
        }
    }
}
