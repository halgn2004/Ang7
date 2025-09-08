using Ang7.ViewModels;
using RGPopup.Maui.Pages;


namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Loading : PopupPage
{
    public Loading(PopUpLoadingViewModel pop)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        BindingContext = pop;
    }
    protected override bool OnBackButtonPressed()
    {
        return false;
    }
}