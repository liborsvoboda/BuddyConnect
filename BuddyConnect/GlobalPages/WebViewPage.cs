using BuddyConnect.Functions;

namespace BuddyConnect
{
    public partial class WebViewPage : ContentPage, GlobalServices {

       // class WebViewPage : ContentPage {

        public WebViewPage() {
            Label header = new Label {
                Text = "WebView",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            WebView webView = new WebView {
                Source = new UrlWebViewSource {
                    Url = "https://kliknetezde.cz/webbt/",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10,20, 10, 5);

            // Build the page.
            this.Content = new StackLayout {
                Children =
                {
                    header,
                    webView
                }
            };

            _ = LoadStartUpData();
        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }

        public async Task<bool> LoadStartUpData() {
            //ti_themeSetting.Text = await SystemFunctions.ChangeorLoadTheme();
            return true;
        }
        //   }

    }
}
