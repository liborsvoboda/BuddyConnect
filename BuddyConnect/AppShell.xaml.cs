using BuddyConect.Resources.Languages;
using BuddyConnect.Functions;
using Microsoft.Maui.Handlers;

namespace BuddyConnect;

public partial class AppShell : Shell
{

	public AppShell()
	{
		InitializeComponent();
    }


    //Change Theme
    private async void OnThemeToolbarItemClicked(object sender, EventArgs e) {
        await SystemFunctions.ChangeorLoadTheme(true);
    }


    //Before Navigation Change
    //Update Or Load Data For Show On Page
    //Central Control Over EachPage LoadStartupData on Page Selection
    private void AfterNavigationChanged(object sender, ShellNavigatedEventArgs e) {
        try {
            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(MainPage).Name) {
                ((MainPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(MainPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.AppName;
            }
            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(NewsListPage).Name) {
                ((NewsListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(NewsListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.News;
            }
            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(SettingListPage).Name) {
                ((SettingListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(SettingListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.Settings;
            }

            
        } catch { }
    }
}
