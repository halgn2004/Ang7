using UIKit;
using Ang7.Helpers;
using Foundation;

namespace Ang7.Platforms.iOS.Helpers
{
    public class RotateService : IRotateInterface
    {
        public void EnableRotation()
        {
            // Allow all orientations (implement orientation logic in AppDelegate if needed)
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Unknown), new NSString("orientation"));
        }

        public void DisableRotation()
        {
            // Lock to portrait (can also be managed in AppDelegate)
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}
