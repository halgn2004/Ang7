using Ang7.Models;
using Ang7.ViewModels;
using Ang7.Views.PopUp;
using Microsoft.Maui.Controls.Shapes;
using Plugin.Maui.ScreenSecurity;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AL_HomePage : ContentPage
{
    public static User CU { get; set; }
    public static Teacher CT { get; set; }
    AL_HomePageViewModel vm;
    private bool isSwipeOpen = false; // flag to track the swipe state
    public AL_HomePage()
    {
        InitializeComponent();
        BindingContext = vm = new AL_HomePageViewModel(Navigation);
        brayad();
#if !WINDOWS
        if (CU.AllowSS)
            ScreenSecurity.Default.DeactivateScreenSecurityProtection();
        else
            ScreenSecurity.Default.ActivateScreenSecurityProtection();
#endif

    }
    private async void brayad()
    {
        if (CU == null)
            await AppShell.GoToPage(nameof(HomePage));
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
        if (vm.IsSearchMode)
            {
                vm.CloseSearchMode();
                searchBar.Text = "";
            }
            else
            {
                CloseSwipe(null, null);
            }
        //});
        return true;
    }
    private void CloseSB(object sender, EventArgs e)
    {
        searchBar.Unfocus();
        searchBar.Text = "";
        vm.CloseSearchMode();
    }
    private async void OpenAnimation()
    {
        await pancake.ScaleYTo(0.9, 300, Easing.SinOut);
        //pancake.CornerRadius = 20;
        pancake.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(20)
        };
        await pancake.RotateTo(-15, 300, Easing.SinOut);
        if (DeviceInfo.Platform == DevicePlatform.iOS)
            await pancake.TranslateTo(-200, 0, 300, Easing.SinOut);
    }

    private async void CloseAnimation()
    {
        await pancake.RotateTo(0, 300, Easing.SinOut);
        pancake.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(0)
        };
        await pancake.ScaleYTo(1, 300, Easing.SinOut);
        if (DeviceInfo.Platform == DevicePlatform.iOS)
            await pancake.TranslateTo(0, 0, 300, Easing.SinOut);
    }

    private void OpenSwipe(object sender, EventArgs e)
    {
        if (isSwipeOpen)
        {
            CloseSwipe(sender, e);
        }
        else
        {
            /*if (DeviceInfo.Platform != DevicePlatform.iOS)
            {
                MainSwipeView.Open(OpenSwipeItem.RightItems);
                OpenAnimation();
            }*/
            MainSwipeView.Open(OpenSwipeItem.RightItems, false);
            OpenAnimation();
            isSwipeOpen = true;
        }
    }

    private void CloseSwipe(object sender, EventArgs e)
    {
        MainSwipeView.Close();
        CloseAnimation();
        isSwipeOpen = false;
    }
    private async void MenuTapped(object sender, EventArgs e)
    {
        string title = ((sender as Microsoft.Maui.Controls.StackLayout).BindingContext as Models.Menu).Name;
        switch (title)
        {
            case "الملف الشخصي":
                await TapEventHandler.HandleTapEvent(false, async () =>
                {
                    await Navigation.PushAsync(new AL_Profile(/*vm.MyOrders*/));
                }, Navigation);
                //CloseSwipe(null, null);
                break;
            case "مدير الملفات":
                await TapEventHandler.HandleTapEvent(false, async () =>
                {
                    await Navigation.PushAsync(new AL_TeacherFilesManger());
                }, Navigation);
                //CloseSwipe(null, null);
                break;
            case "الإعدادات":
                await TapEventHandler.HandleTapEvent(false, async () =>
                {
                    await Navigation.PushAsync(new AL_Settings());
                }, Navigation);
                //CloseSwipe(null, null);
                break;
            case "المقترحات والشكاوي":
                await TapEventHandler.HandleTapEvent(false, async () =>
                {
                    if (CU.UserType == 1)
                        if (vm.AllFBs != null && vm.AllFBs.Count > 0)
                            await Navigation.PushAsync(new AdminShowFeedbacks(vm.AllFBs));
                        else
                            await PopupNavigation.Instance.PushAsync(new Msg("لا يوجد اي مقترحات او شكاوي حاليا.", "ملاحظة", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                    else
                        await PopupNavigation.Instance.PushAsync(new PopUp.Feedback(new FeedbackViewModel()));
                }, Navigation);
                //CloseSwipe(null, null);
                break;
            case "تسجيل خروج":
                await TapEventHandler.HandleTapEvent(false, async () =>
                {
                    if(CU.UserType == -1)
                    {
                        CU = null;
                        //await Navigation.PushAsync(new HomePage());
                        await AppShell.GoToPage(nameof(HomePage));
                        //await Consts.navto(Routes.HomePage, clearStack: true);
                        return;
                    }
                    if (await ConfirmMsg.ConfirmMessage("هل أنت متأكد أنك تود تسجيل الخروج ؟", "تأكيد:", "لا", "نعم", Navigation))
                    {
                        CU = null;
                        await AppShell.GoToPage(nameof(HomePage));
                        //await Consts.navto(Routes.HomePage, clearStack: true);
                    }
                }, Navigation);
                break;
            default:
                break;
        }
    }

    protected override void OnAppearing()
    {
        vm.OnAppering();
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        vm.PerformSearchCommand.Execute(e.NewTextValue);
    }

}