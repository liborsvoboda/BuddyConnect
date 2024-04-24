using SQLite;

namespace BuddyConnect;

public partial class App : Application {


	//Central Declaration
    public static AppSetting AppSetting = new AppSetting();


    public App()
	{
        InitializeComponent();
        MainPage = new AppShell();
	}
}
