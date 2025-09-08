using Android.Content.PM;
using Ang7.Helpers;

namespace Ang7.Platforms.Android.Helpers;

public class RotateService : IRotateInterface
{
    public void EnableRotation()
    {
        // Allow rotation
        var activity = Platform.CurrentActivity;
        activity.RequestedOrientation = ScreenOrientation.Unspecified;
    }

    public void DisableRotation()
    {
        // Lock to portrait mode
        var activity = Platform.CurrentActivity;
        activity.RequestedOrientation = ScreenOrientation.Portrait;
    }
}
