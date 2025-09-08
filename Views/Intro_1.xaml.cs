namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Intro_1 : ContentPage
{
    public Intro_1()
    {
        InitializeComponent();
        /*if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
        {
            // Remove HeightRequest for tablets (use the default auto height)
            LKIMG.HeightRequest = -1; // Or leave it undefined
            LKIMG.MaximumHeightRequest = 800;
        }*/
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
/*#if IOS
        UIKit.UINavigationController vc = (UIKit.UINavigationController)Platform.GetCurrentUIViewController();//using UIKit, find the UINavigationController  
        vc.InteractivePopGestureRecognizer.Enabled = false;  
#endif*/
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        //Consts.navto(nameof(Intro_2), false);

        _ = AppShell.GoToPage(nameof(Intro_2));
        //await Consts.navto(Routes.Intro_2, clearStack: false);
        /*await TapEventHandler.HandleTapEvent(false, async () =>
        {
            await Navigation.PushAsync(new Intro_2(), true);
        }, Navigation);*/
    }
}