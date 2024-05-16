using Foundation;
using UIKit;

namespace BuddyConnect;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) {
        return base.FinishedLaunching(application, launchOptions);
    }
}
