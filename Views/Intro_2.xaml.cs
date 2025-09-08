namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Intro_2 : ContentPage
{
    public Intro_2()
    {
        InitializeComponent();
        /*if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
        {
            // Remove HeightRequest for tablets (use the default auto height)
            LKIMG.HeightRequest = -1; // Or leave it undefined
            LKIMG.MaximumHeightRequest = 800;
        }*/
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        //Consts.navto(nameof(HomePage) , true);
        //await Consts.navto(Routes.HomePage, clearStack: true);

        _ = AppShell.GoToPage(nameof(HomePage));
        /*await TapEventHandler.HandleTapEvent(false, async () =>
        {
            await Navigation.PushAsync(new HomePage(), true);
        }, Navigation);*/
    }
}
//ctrl + K +D