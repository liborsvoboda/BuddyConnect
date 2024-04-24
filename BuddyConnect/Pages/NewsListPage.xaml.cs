using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class NewsListPage : ContentPage
    {
        public NewsListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }


        public async Task<bool> LoadStartUpData() {
            //ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme();
            return true;
        }


    }
}
