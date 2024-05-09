using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class NewsListPage : ContentPage, GlobalServices
    {
        public NewsListPage() {
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


    }
}
