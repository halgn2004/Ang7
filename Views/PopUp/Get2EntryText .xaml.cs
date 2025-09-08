using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Get2EntryText : PopupPage
{
    private readonly Action<string, string> setResultAction;
    public Get2EntryText(string msg, string placeholder, string placeholder2, int ml, int keyboardtype, string nobut, string yesbut, Action<string, string> sra)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        HeadLabel.Text = msg;
        entry.Placeholder = placeholder;
        entry2.Placeholder = placeholder2;
        entry.MaxLength = ml;
        if (keyboardtype == 2)
            entry.Keyboard = Keyboard.Telephone;
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
        setResultAction?.Invoke("c" , "c");
        return false;
    }

    public void OnNoButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke("c" , "c");
    }
    public void OnYesButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        if (entry.Text == null || entry2.Text == null)
            setResultAction?.Invoke("","");
        else
        {
            string text1 = entry.Text ?? "";
            string text2 = entry2.Text ?? "";

            setResultAction?.Invoke(text1, text2);
        }
    }
    public static async Task<(string, string)> Get2EntryTxt(string title, string placeholder, string placeholder2, int ml, int keyboardtype, string nobut, string yesbut, INavigation nav)
    {
        TaskCompletionSource<(string, string)> cs = new TaskCompletionSource<(string, string)>();
        void callback(string text1, string text2)
        {
            cs.TrySetResult((text1, text2));
        }
        var pop = new Get2EntryText(title, placeholder, placeholder2, ml, keyboardtype, nobut, yesbut, callback);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}