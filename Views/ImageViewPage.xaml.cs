namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
//[QueryProperty(nameof(LKTitle), "title")]
//[QueryProperty(nameof(IS), "Source")]
//[QueryProperty(nameof(From), "from")]

public partial class ImageViewPage : ContentPage
{
    /*public string LKTitle
    {
        set
        {
            Title.Text = value.ToString();
        }
    }
    public string IS
    {
        set
        {
            ImageView.Source = value.ToString();
            ImageView.Success += OnImageLoaded;
            ImageView.Error += OnImageLoadFailed;
        }
    }
    public string From { get; set; }*/
    
    string mylink;
    public ImageViewPage(string t, string l)
    {
        InitializeComponent();
        WatermarkView.BadgeText = AL_HomePage.CU.UserID.ToString();
        //ScreenSecurity.Default.ScreenCaptured += OnStartSS;
        Title.Text = t;
        ImageView.Source = mylink = l;
        //ImageView.Success += OnImageLoaded;
        ImageView.Error += OnImageLoadFailed;
    }
    private void OnImageLoaded(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
/*#if IOS
        ImageView.Source = new UriImageSource
        {
            Uri = new Uri($"{mylink}?t={DateTime.Now.Ticks.ToString()}")
        };
#endif*/
    }

    private void OnImageLoadFailed(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
    }

    /*private void OnStartSS(object sender, EventArgs e)
    {
        //AppShell.GoToPage(nameof(HomePage));
        Console.WriteLine("byscrren ");
    }*/

    protected override bool OnBackButtonPressed()
    {
        Navigation.PopAsync();
        return true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        //AppShell.GoToPage("..");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WatermarkView.StopAnimations();
        //ImageView.Success -= OnImageLoaded;
        ImageView.Error -= OnImageLoadFailed;
        //ScreenSecurity.Default.ScreenCaptured -= OnStartSS;
    }
}