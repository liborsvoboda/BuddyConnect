using BuddyConnect.Resources.Languages;
using BuddyConnect.Controllers;
using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BuddyConnect.Functions
{
    public class SystemFunctions {

        //Central Change Theme
        public async static Task<string> ChangeorLoadTheme(bool change = false) {
            string theme = null; 
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null) { mergedDictionaries.Clear(); }

            
            if (string.IsNullOrWhiteSpace(theme) && App.appSetting.Settings.Where(a=>a.Key == "Theme").First().Value == null) {
                //Load from DB
                theme = (await SettingListController.LoadSettingListThemeStartup()).Value;
                change = false;
            }
            else if (string.IsNullOrWhiteSpace(theme) && App.appSetting.Settings.Where(a => a.Key == "Theme").First().Value != null && !change) {
                //Load from Setting
                theme = App.appSetting.Settings.Where(a => a.Key == "Theme").First().Value;
            }
            else if (string.IsNullOrWhiteSpace(theme) && App.appSetting.Settings.Where(a => a.Key == "Theme").First().Value != null && change) {
                //change Theme
                theme = App.appSetting.Settings.Where(a => a.Key == "Theme").First().Value;
            }

            if (!change) {
                switch (theme) {
                    case "Light":
                        theme = "Light"; 
                        mergedDictionaries.Add(new LightTheme()); 
                        break;
                    case "Dark":
                    default:
                        theme = "Dark"; 
                        mergedDictionaries.Add(new DarkTheme()); 
                        break;
                }
            }
            else {
                switch (theme) {
                    case "Light":
                        theme = "Dark"; 
                        mergedDictionaries.Add(new DarkTheme()); 
                        break;
                    case "Dark":
                    default:
                        theme = "Light";
                        mergedDictionaries.Add(new LightTheme()); 
                        break;
                }
                await SettingListController.SetSelectedTheme(theme);
            }
            return theme;
        }


        //Central Change Language
        public async static Task<string> ChangeorLoadLanguage(string language = null) {
            //Load from DB
            if (string.IsNullOrWhiteSpace(language)) {
                language = (await SettingListController.LoadSettingListLanguageStartup()).Value;
                //Load from Setting
            }

            //Save Selected Dictionary
            await SettingListController.SetSelectedLanguage(language);

            //Set Selected Dictionary
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            AppResources.Culture = new CultureInfo(language);

            return App.appSetting.Settings.Where(a => a.Key == "Language").First().Value;
        }


        /// <summary>
        /// Extension For Checking Operation System of Server Running
        /// </summary>
        public static class GetOperatingSystemInfo {

            public static bool IsWindows() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            public static bool IsMacOS() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            public static bool IsLinux() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        }


        /// <summary>
        /// Mined-ed Error Message For Answer in API Error Response with detailed info about problem
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static string GetUserApiErrMessage(Exception exception, int msgCount = 1) {
            string result = exception != null ? string.Format("{0}: {1}\n{2}", msgCount, exception.Message, GetUserApiErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
            #if DEBUG
            Debug.WriteLine(result);
            #endif
            return result;

        }

        /// <summary>
        /// Mined-ed Error Message For System Save to Database For Simple Solving Problem
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="msgCount"> </param>
        /// <returns></returns>
        public static string GetSystemErrMessage(Exception exception, int msgCount = 1) {
            string result = exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetSystemErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
            #if DEBUG
            Debug.WriteLine(result);
            #endif
            return result;
        }

    }

}
