using Ang7.Views.PopUp;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminShowFeedbacks : ContentPage
	{
    public ObservableCollection<Models.Feedback> Myfbs { get; set; }
    public AdminShowFeedbacks(ObservableCollection<Models.Feedback> us)
    {
        InitializeComponent();
        Myfbs = us;
        this.BindingContext = this;
    }

    private void Onback_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        if (PopupNavigation.Instance.PopupStack.Any())
        {
            // Close the current popup
            TapEventHandler.Reset_isNavigating();
            PopupNavigation.Instance.PopAsync();
            return true; // Return true to prevent the back press from affecting the main page navigation
        }

        //Device.BeginInvokeOnMainThread(() => {
        Navigation.PopAsync();
        //});
        return true;
    }


    private async Task OpenAnimation(View view, uint length = 250)
    {
        view.RotationX = -90;
        view.IsVisible = true;
        view.Opacity = 0;
        _ = view.FadeTo(1, length);
        await view.RotateXTo(0, length);
    }

    private async Task CloseAnimation(View view, uint length = 250)
    {
        _ = view.FadeTo(0, length);
        await view.RotateXTo(-90, length);
        view.IsVisible = false;
    }

    private async void MainExpander_Tapped(object sender, EventArgs e)
    {
        var expander = sender as Expander;
        var View = expander.FindByName<Label>("DetailsView");
        var uView = expander.FindByName<Microsoft.Maui.Controls.Grid>("uDetailsView");

        if (expander.IsExpanded)
        {
            await OpenAnimation(View);
            await OpenAnimation(uView);
        }
        else
        {
            await CloseAnimation(uView);
            await CloseAnimation(View);
        }
    }

    private async void OnCompleted(object sender, EventArgs e)
    {
        var u = ((ImageButton)sender).BindingContext as Models.Feedback;

        if (await ConfirmMsg.ConfirmMessage("هل انت متأكد انك تريد أرسال هذا الرد ؟", "تأكيد:", "لا", "نعم", Navigation))
        {
            var json = await GlobalFunc.Admin_SubmitCompleteFeedBack(AL_HomePage.CU.UserID, u.ID, (string)((ImageButton)sender).CommandParameter);
            if (json == "3ash.")
            {
                //await PopupNavigation.Instance.PushAsync(new Msg($"تم قبول {u.ShopName} بنجاح.", "تجاح", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
                Myfbs.Remove(u);
            }
        }
    }


}