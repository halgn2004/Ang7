using System.Collections.ObjectModel;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Select : PopupPage
{
    private readonly Action<string> setResultAction;
    public Select(string title, ObservableCollection<PopUpSelectItemModel> myitems, Action<string> sra, bool IsDelBut)
    {
        InitializeComponent();
        //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        HeadLabel.Text = title;
        Delbut.IsVisible = IsDelBut;
        Items.ItemsSource = myitems;
        Items.HeightRequest = myitems.Count * 50 + 5;
        setResultAction = sra;
    }
    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }

    private void OnClose_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke("cancel");
    }

    private void Ondelbut_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke("delete");
    }

    private void Items_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke((e.Item as PopUpSelectItemModel).Desc);
    }
    public static async Task<string> SelectMessage(string title, ObservableCollection<PopUpSelectItemModel> myitems, bool IsDelBut, INavigation nav)
    {
        TaskCompletionSource<string> cs = new TaskCompletionSource<string>();
        void callback(string didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new Select(title, myitems, callback, IsDelBut);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}
public class PopUpSelectItemModel
{
    public string Img { get; set; }
    public string Desc { get; set; }

}