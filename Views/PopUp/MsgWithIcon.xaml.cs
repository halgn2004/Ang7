using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MsgWithIcon : PopupPage
{
    private readonly Action<bool> setResultAction;

    public MsgWithIcon(Action<bool> sra,string image, string msg, string buttext, 
        string ButBG = "#DB0D0D", string ButTxtColor = "#000000")
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        setResultAction = sra;
        ErrorImage.Source = ImageSource.FromFile(image);
        MsgLabel.Text = msg;
        SubmitButton.Text = buttext;
        SubmitButton.BackgroundColor = Color.FromArgb(ButBG);
        SubmitButton.BorderColor = Color.FromArgb(ButBG);
        SubmitButton.TextColor = Color.FromArgb(ButTxtColor);
    }
    private void OnButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(true);
    }
    public static async Task<bool> ShowNoConn(INavigation nav)
    {
        TaskCompletionSource<bool> cs = new TaskCompletionSource<bool>();
        void callback(bool didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new MsgWithIcon(callback, "nowifi.png", "لا يوجد أتصال بالأنترنت" , "محاولة أخري");
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
    public static async Task<bool> ShowError(string msg, INavigation nav, string buttext = "محاولة أخري")
    {
        TaskCompletionSource<bool> cs = new TaskCompletionSource<bool>();
        void callback(bool didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new MsgWithIcon(callback, "popupmsgerror.png", msg, buttext);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
    public static async Task<bool> ShowUpToDate(INavigation nav)
    {
        TaskCompletionSource<bool> cs = new TaskCompletionSource<bool>();
        void callback(bool didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new MsgWithIcon(callback, "popupmsguptodate.png", "التطبيق أحدث إصدار", "موافق" , "#248d35", "#ffffff");
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }

}