using Ang7.ViewModels;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ForgotPassword : PopupPage
{
    public ForgotPassword(PopUpForgotPasswordViewModel pop)
    {
        InitializeComponent();
        BindingContext = pop;
    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }
    private void OnClose_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
    }

}