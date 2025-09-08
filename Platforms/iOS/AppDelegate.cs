using Foundation;
using UIKit;

namespace Ang7;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // Set status bar content to dark (icons and text)
        UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.DarkContent;

        return base.FinishedLaunching(application, launchOptions);
    }
}
