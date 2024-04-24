using BuddyConnect.Functions;
using System.Diagnostics;

namespace BuddyConnect
{
    public partial class SettingListPage : ContentPage, IModalPage {
        
        public SettingListPage() {
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
