using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Msg : PopupPage
{
    public Msg(string msg, string Label, Color c, string okbut)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        if (string.IsNullOrEmpty(Label))
        {
            msgview.IsVisible = false;
            HeadLabel.HorizontalTextAlignment = TextAlignment.Center;
            HeadLabel.Text = msg;
            HeadLabel.TextColor = c;
        }
        else
        {
            HeadLabel.Text = Label;
        }
        MsgLabel.Text = msg;
        OKBut.Text = okbut;
    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }
    public void OnOKButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
    }

}