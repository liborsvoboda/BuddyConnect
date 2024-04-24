using BuddyConnect.Functions;
using System.Diagnostics;

namespace BuddyConnect
{
    public partial class SettingListPage : ContentPage {
        
        public SettingListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
        }


        public async Task<bool> LoadStartUpData() {
            //ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme();
            return true;
        }


    }
}
