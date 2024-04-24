
namespace BuddyConnect
{
    public partial class ThemeSelectionPage : ContentPage, IModalPage
    {
        public ThemeSelectionPage()
        {
            InitializeComponent();
        }
        /*
        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            string theme = (Themes)picker.SelectedItem;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case Themes.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case Themes.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
                statusLabel.Text = $"{theme.ToString()} theme loaded. Close this page.";
            }
        }
        */
        public async Task Dismiss()
        {
            await Navigation.PopModalAsync();
        }
    }
}
