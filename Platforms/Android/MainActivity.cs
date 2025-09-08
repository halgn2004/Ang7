using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Ang7;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        /* Set status bar color
#pragma warning disable CA1422 // Validate platform compatibility
        Window.SetStatusBarColor(Android.Graphics.Color.White);
#pragma warning restore CA1422 // Validate platform compatibility

        // Set status bar content style (light or dark)
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CA1416 // Validate platform compatibility
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CS0618 // Type or member is obsolete
        */
        // Set status bar background color (light gray for visibility)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#F0F0F0"));
        }

        // Set status bar content style (default dark text/icons)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            Window?.InsetsController?.Show(WindowInsets.Type.StatusBars());
        }
        else if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
