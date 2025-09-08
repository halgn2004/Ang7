using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ImageViewer : PopupPage
{
    public ImageViewer(string N,string L)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        WatermarkView.BadgeText = AL_HomePage.CU.UserID.ToString();
        Title.Text = N;
        ImageView.Source = L;
        ImageView.Success += OnImageLoaded;
        ImageView.Error += OnImageLoadFailed;

    }

    private void OnImageLoaded(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
    }

    private void OnImageLoadFailed(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        
    }
    protected override void OnDisappearing()
    {

        WatermarkView.StopAnimations();
        base.OnDisappearing();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //Controls.FullScreen();
        //ServiceLocator.GetService<IRotateInterface>().EnableRotation();
    }

}