using Ang7.Helpers;
using Plugin.Maui.ScreenSecurity;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;
#if IOS
using AVKit;
#endif
namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class VideoPlayer : PopupPage
{
    public VideoPlayer(string t,string videoSource)
    {
        InitializeComponent();
        videoPlayer.Source = videoSource;
        Title.Text = t;
        WatermarkView.BadgeText = AL_HomePage.CU.UserID.ToString();
#if !WINDOWS
        ScreenSecurity.Default.ActivateScreenSecurityProtection();
#endif

#if IOS
        var avPlayerViewController = videoPlayer.Handler?.PlatformView as AVPlayerViewController;
        
                Console.WriteLine("lk1");
        if (avPlayerViewController != null)
        {
                Console.WriteLine("lk2");
            // Access the AVPlayer from the AVPlayerViewController
            var avPlayer = avPlayerViewController.Player;

            // Disable AirPlay (external playback)
            if (avPlayer != null)
            {
                Console.WriteLine("lk3");
                avPlayer.AllowsExternalPlayback = false;
            }
        }
#endif

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ServiceLocator.GetService<IRotateInterface>().EnableRotation();
    }

    private async void VideoPlayer_PlayCompletion(object sender, EventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        PopupNavigation.Instance.PopAsync();
        return true;
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        //await Navigation.PopAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        videoPlayer.Pause();
        WatermarkView.StopAnimations();
        ServiceLocator.GetService<IRotateInterface>().DisableRotation();

    }
}