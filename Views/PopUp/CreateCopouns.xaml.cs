using Ang7.ViewModels;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CreateCopouns : PopupPage
{
    public CreateCopouns(/*PopUpCreateCopounsViewModel pop*/ int fid)
    {
        InitializeComponent();
        BindingContext = new PopUpCreateCopounsViewModel(fid);
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