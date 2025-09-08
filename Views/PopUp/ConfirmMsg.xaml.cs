using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConfirmMsg : PopupPage
{
    private readonly Action<bool> setResultAction;
    public ConfirmMsg(string msg, string Label, string nobut, string yesbut, Action<bool> sra)
    {
        InitializeComponent();
        MsgLabel.Text = msg;
        HeadLabel.Text = Label;
        NoBut.Text = nobut;
        YesBut.Text = yesbut;
        if (nobut.Length >= 4 || yesbut.Length >= 4)
        {
            NoBut.FontSize = 14;
            YesBut.FontSize = 14;
        }
        setResultAction = sra;
    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(false);
        return false;
    }

    public void OnNoButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(false);
    }
    public void OnYesButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(true);

    }

    public static async Task<bool> ConfirmMessage(string msg, string Label, string nobut, string yesbut, INavigation nav)
    {
        TaskCompletionSource<bool> cs = new TaskCompletionSource<bool>();
        void callback(bool didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new ConfirmMsg(msg, Label, nobut, yesbut, callback);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}