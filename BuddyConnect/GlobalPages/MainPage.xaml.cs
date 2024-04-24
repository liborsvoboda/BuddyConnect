using BuddyConnect.Functions;
using System.Diagnostics;

namespace BuddyConnect;

public partial class MainPage : ContentPage, IModalPage
{
	int count = 0;

	public MainPage() {
		InitializeComponent();
        _ = LoadStartUpData();
    }


    public async Task<bool> LoadStartUpData() {
        return true;
    }


    public async Task Dismiss() {
        await Navigation.PopModalAsync();
    }


    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

