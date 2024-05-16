using BuddyConnect.Functions;
using BuddyConnect.Resources.Languages;

namespace BuddyConnect
{
    public partial class WebViewPage : ContentPage, GlobalServices {


        public WebViewPage() {
         
            _ = LoadStartUpData();
        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }

        public async Task<bool> LoadStartUpData() {

            Label header = new Label {
                Text = AppResources.WebViewPage,
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center
            };

            WebView webView = new WebView {
                Source = new UrlWebViewSource {
                    Url = App.appSetting.Settings.Where(a => a.Key == "WebPage").FirstOrDefault().Value,
                },
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, 20, 10, 5);

            // Build the page.
            this.Content = new StackLayout {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Children = { header, webView }
            };
            Content.AddLogicalChild(webView);
            TranslatePageObjects();
            return true;
        }


        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {

        }
    }
}
