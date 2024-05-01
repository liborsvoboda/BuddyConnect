using BuddyConnect.Resources.Languages;
using BuddyConnect.Controllers;
using System.Reflection;
using System.Globalization;

namespace BuddyConnect.Functions
{
    public class SystemFunctions {

        //Central Change Theme
        public async static Task<string> ChangeorLoadTheme(bool change = false) {
            string theme = null; string translatedTheme = null;
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null) { mergedDictionaries.Clear(); }

            //Load from DB
            if (string.IsNullOrWhiteSpace(theme) && App.AppSetting.Theme == null) {
                theme = (await SettingListController.LoadSettingListThemeStartup()).Value;
                change = false;
                //Load from Setting
            }
            else if (string.IsNullOrWhiteSpace(theme) && App.AppSetting.Theme != null && !change) {
                theme = App.AppSetting.Theme;

                //change Theme
            }
            else if (string.IsNullOrWhiteSpace(theme) && App.AppSetting.Theme != null && change) {
                theme = App.AppSetting.Theme;
            }

            if (!change) {
                switch (theme) {
                    case "Light":
                        theme = "Light"; translatedTheme = App.AppSetting.TranslatedTheme = AppResources.Dark;
                        mergedDictionaries.Add(new LightTheme()); break;
                    case "Dark":
                    default:
                        theme = "Dark"; translatedTheme = App.AppSetting.TranslatedTheme = AppResources.Light;
                        mergedDictionaries.Add(new DarkTheme()); break;
                }
            }
            else {
                switch (theme) {
                    case "Light":
                        theme = "Dark"; translatedTheme = App.AppSetting.TranslatedTheme = AppResources.Light;
                        mergedDictionaries.Add(new DarkTheme()); break;
                    case "Dark":
                    default:
                        theme = "Light"; translatedTheme = App.AppSetting.TranslatedTheme = AppResources.Dark;
                        mergedDictionaries.Add(new LightTheme()); break;
                }
                await SettingListController.SetSelectedTheme(theme);
            }
            return translatedTheme;
        }


        //Central Change Language
        public async static Task<string> ChangeorLoadLanguage(string language = null) {
            //Load from DB
            if (string.IsNullOrWhiteSpace(language)) {
                language = (await SettingListController.LoadSettingListLanguageStartup()).Value;
                //Load from Setting
            }

            //Save Selected Dictionary
            App.AppSetting.Language = language;
            await SettingListController.SetSelectedLanguage(language);

            //Set Selected Dictionary
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            AppResources.Culture = new CultureInfo(language);

            return App.AppSetting.Language;
        }
    }
}
