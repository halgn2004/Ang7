using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GetEditorText : PopupPage
{
    private readonly Action<string> setResultAction;
    public GetEditorText (string msg, string placeholder, int ml, int keyboardtype, string nobut, string yesbut, Action<string> sra)
		{
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        HeadLabel.Text = msg;
        editor.Placeholder = placeholder;
        editor.MaxLength = ml;
        if (keyboardtype == 2)
            editor.Keyboard = Keyboard.Telephone;
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
        setResultAction?.Invoke("c");
        return false;
    }
    public void OnNoButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke("c");
    }
    public void OnYesButClick(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        if (editor.Text == null)
            setResultAction?.Invoke("");
        else
            setResultAction?.Invoke(editor.Text);
    }
    public static async Task<string> GetEditorTxt(string title, string placeholder, int ml, int keyboardtype, string nobut, string yesbut, INavigation nav)
    {
        TaskCompletionSource<string> cs = new TaskCompletionSource<string>();
        void callback(string didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new GetEditorText(title, placeholder, ml, keyboardtype, nobut, yesbut, callback);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}