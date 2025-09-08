using Ang7.Views.PopUp;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FeedbackMsg : ContentPage
{
    public ObservableCollection<Models.Feedback> Myfbs { get; set; }

    public FeedbackMsg(ObservableCollection<Models.Feedback> us)
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

        Navigation.PopAsync();
        return true;  // Ensures that the back button doesn't function as default
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
        var view = expander.FindByName<Grid>("DetailsView");

        if (expander.IsExpanded)
        {
            await OpenAnimation(view);
        }
        else
        {
            await CloseAnimation(view);
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
                Myfbs.Remove(u);
            }
        }
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeview = sender as SwipeItem;
        var myfb = (Models.Feedback)swipeview.CommandParameter;
        var json = await GlobalFunc.APIDelFB(AL_HomePage.CU.UserID, myfb.ID);
        if (json.Substring(0, 5) != "error")
        {
            Myfbs.Remove(myfb);
        }
    }
}
