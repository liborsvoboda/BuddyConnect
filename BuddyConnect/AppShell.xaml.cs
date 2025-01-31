﻿using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;


namespace BuddyConnect;

public partial class AppShell : Shell {

    public AppShell() {
        InitializeComponent();
        _ = LoadStartUpData();
    }


    //Load App Startup
    public async Task<bool> LoadStartUpData() {
        await StatupControls.StartupInit();
        language.Source = ImageSource.FromFile(await SystemFunctions.ChangeorLoadLanguage() + ".png");

        //RENAME MENU ITEMS
        SetTranslatedTitle();
        return true;
    }


    //Change Theme
    private async void OnThemeToolbarItemClicked(object sender, EventArgs e) {
        await SystemFunctions.ChangeorLoadTheme(true);
    }


    //change Language
    private async void SelectLanguageClicked(object sender, EventArgs e) {
        string action = await DisplayActionSheet(AppResources.LanguageSelection, AppResources.Cancel, null, AppResources.Czech, AppResources.English, AppResources.Deutsch);

        if (action == AppResources.Czech) { language.Source = ImageSource.FromFile(await SystemFunctions.ChangeorLoadLanguage("cs") + ".png"); }
        else if (action == AppResources.English) { language.Source = ImageSource.FromFile(await SystemFunctions.ChangeorLoadLanguage("en") + ".png"); }
        else if (action == AppResources.Deutsch) { language.Source = ImageSource.FromFile(await SystemFunctions.ChangeorLoadLanguage("de") + ".png"); }

        SetTranslatedTitle();
        await LayoutPagesStartup();
    }



    //Before Navigation Change
    //Update Or Load Data For Show On Page
    //Central Control Over EachPage LoadStartupData on Page Selection
    private async void AfterNavigationChanged(object sender, ShellNavigatedEventArgs e) => await LayoutPagesStartup();

    private async Task<bool> LayoutPagesStartup() {
        try {
            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(DeviceManagementPage).Name) {
                await ((DeviceManagementPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(DeviceManagementPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.DeviceManagement;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(WebViewPage).Name) {
                await ((WebViewPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(WebViewPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.WebViewPage;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(NewsListPage).Name) {
                await ((NewsListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(NewsListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.News;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(SettingListPage).Name) {
                await ((SettingListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(SettingListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.Settings;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(NoteListPage).Name) {
                await ((NoteListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(NoteListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.Notes;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(ContactListPage).Name) {
                await ((ContactListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(ContactListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.Contacts;
            }

            if (((Shell)App.Current.MainPage).CurrentPage.GetType().Name == typeof(AboutListPage).Name) {
                await ((AboutListPage)((Shell)((Shell)App.Current.MainPage).Items.First(a => a.CurrentItem.Route.Contains(typeof(AboutListPage).Name)).CurrentItem.Window.Page).CurrentPage).LoadStartUpData();
                selectedMenu.Text = AppResources.About;
            }

        } catch { }
        return true;
    }



    //Change/Set  Translated Page Name
    private void SetTranslatedTitle() {
        WelcomePage.Title = AppResources.Welcome;
        WebViewPage.Title = AppResources.WebViewPage;
        DeviceManagementPage.Title = AppResources.DeviceManagement;
        NewsListPage.Title = AppResources.News;
        SettingListPage.Title = AppResources.Settings;
        NoteListPage.Title = AppResources.Notes;
        ContactListPage.Title = AppResources.Contacts;
        AboutListPage.Title = AppResources.About;
    }


}
