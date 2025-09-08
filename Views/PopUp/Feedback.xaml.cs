using Ang7.ViewModels;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Feedback : PopupPage
{
    public Feedback(FeedbackViewModel pop)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        BindingContext = pop;
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
}