using Ang7.ViewModels;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectAvatar : PopupPage
{
    PopUpSelectAvatarViewModel vm;
    private readonly Action<string> setResultAction;
    public SelectAvatar(Action<string> act)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        BindingContext = vm = new PopUpSelectAvatarViewModel();
        this.setResultAction = act;

    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }
    public static async Task<string> SAvatar(INavigation nav)
    {
        TaskCompletionSource<string> cs = new TaskCompletionSource<string>();
        void callback(string didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new SelectAvatar(callback);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }

    private void OnSave(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(string.IsNullOrEmpty(vm.SelectedAvatar)? "cancel" : vm.SelectedAvatar);
    }

    private void OnCancel(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke("cancel");
    }
}