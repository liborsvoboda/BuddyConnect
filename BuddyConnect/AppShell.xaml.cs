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


    //Language Selection
    private async void SelectLanguageClicked(object sender, EventArgs e) {
        string action = await DisplayActionSheet(AppResources.LanguageSelection, AppResources.Cancel, null, AppResources.Czech, AppResources.English, AppResources.Deutsch);

        if (action == AppResources.Czech) { language.Source = ImageSource.FromFile("cz.png"); }
        else if (action == AppResources.English) { language.Source = ImageSource.FromFile("en.png"); }
        else if (action == AppResources.Deutsch) { language.Source = ImageSource.FromFile("de.png"); }
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

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(NoteListPage).Name) {
                ((NoteListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(NoteListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.Notes;
            }
            
            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(AboutListPage).Name) {
                ((AboutListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(AboutListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.About;
            }
            
        } catch { }
    }


}
