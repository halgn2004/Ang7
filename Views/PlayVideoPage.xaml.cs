using Ang7.Helpers;
using RGPopup.Maui.Services;
namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
//[QueryProperty(nameof(LKTitle), "title")]
//[QueryProperty(nameof(VS), "Source")]
//[QueryProperty(nameof(From), "from")]

public partial class PlayVideoPage : ContentPage
{
    /*public string LKTitle
    {
        set
        {
            Title.Text = value.ToString();
        }
    }
    public string VS
    {
        set
        {
            videoPlayer.Source = value.ToString();
        }
    }
    public string From { get; set; }*/
    public PlayVideoPage(string t,string videoSource)
    {
        InitializeComponent();
        videoPlayer.Source = videoSource;
        Title.Text = t;
        WatermarkView.BadgeText = AL_HomePage.CU.UserID.ToString();
      
        if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Any())
            PopupNavigation.Instance.PopAsync();
        //ScreenSecurity.Default.ScreenCaptured += OnStartSS;
    }
    /*private void OnStartSS(object sender, EventArgs e)
    {
        //AppShell.GoToPage(nameof(HomePage));
        Console.WriteLine("byscrren ");
        GlobalFunc.ToastShow($"byscrren");
    }*/
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ServiceLocator.GetService<IRotateInterface>().EnableRotation();
    }

    private async void VideoPlayer_PlayCompletion(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        Navigation.PopAsync();
        return true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        //await AppShell.GoToPage("..");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        videoPlayer.Pause();
        WatermarkView.StopAnimations();
        //ScreenSecurity.Default.ScreenCaptured -= OnStartSS;
        ServiceLocator.GetService<IRotateInterface>().DisableRotation();
    }
}