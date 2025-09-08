using Ang7.Models;
using Ang7.ViewModels;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        Helpers.Settings.GeneralSettings_KeepMeLoggedIn = false;
        AL_HomePage.CU = null;
        if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
        {
            // Remove HeightRequest for tablets (use the default auto height)
            SignupLabel0.FontSize = 24;
            SignupLabel.FontSize = 24;
            GuestLabel.FontSize = 24;
        }

    }
    
    protected override bool OnBackButtonPressed()
    {
        if (PopupNavigation.Instance.PopupStack.Any())
        {
            TapEventHandler.Reset_isNavigating();
            PopupNavigation.Instance.PopAsync();
        }

        return true;
    }
    private async void OnSettings_Clicked(object sender, EventArgs e)
    {
        /*if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                // Ensure no pending navigation before launching the camera
                await Task.Delay(100);

                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    using var stream = await photo.OpenReadAsync();
                    // Handle photo stream
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Camera error: {ex.Message}");
            }
        }*/
        await AppShell.GoToPage(nameof(SettingsPage));
        /*await TapEventHandler.HandleTapEvent(false, async () =>
        {
            await Navigation.PushAsync(new SettingsPage(DependencyService.Get<IAppRating>()));
            
        }, Navigation);*/
    }
    private async void OnSignin_Clicked(object sender, EventArgs e)
    {
        await AppShell.GoToPage(nameof(SigninPage));
        //await Consts.navto(Routes.SigninPage);
        //Consts.navto(nameof(SigninPage));
        /*await TapEventHandler.HandleTapEvent(true, async () =>
        {
            await Navigation.PushAsync(new SigninPage());
        }, Navigation);*/
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        /*foreach (var page in Navigation.NavigationStack.Reverse().ToList())
        {
            // Check the type of each page instead of using ToString()
            if (page is AL_HomePage || page is SigninPage || page is AL_Profile || page is SignupPage || page is SS || page is Intro_1 || page is Intro_2)
            {
                Navigation.RemovePage(page);
            }
        }*/
    }

    private async void OnGuestTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            AL_HomePage.CU = new User { UserID=-1, Name="Guest", UserType=-1 , PP= "nouserpic.png"};

            await AppShell.GoToPage(nameof(AL_HomePage));
            //await Consts.navto(Routes.AL_HomePage);
            //await Navigation.PushAsync(new AL_HomePage());
        }, Navigation);
    }

    private async void OnSignupTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var can = await GlobalFunc.CheckPermissionsAndStart();
            if (can)
            {
                PopUpLoadingViewModel _PopUpLoading = new PopUpLoadingViewModel(Navigation);
                var code = await GetEntryText.GetEntryTxt("من فضلك ادخل كود الدعوة", "كود الدعوة", 500, 1, "الغاء", "موافق", Navigation);
                if (code != "c")
                {
                    if (string.IsNullOrEmpty(code) || code.Length < 3)
                        return;

                    await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));
                    string rv = await GlobalFunc.CheckSignupCode(code);

                    if (rv == "error01")
                    {
                        _PopUpLoading.ShowMsg("كود الدعوة غير صحيح.", "حاول مره أخري");
                        return;
                    }
                    else if (rv == "erorr404")
                    {
                        _PopUpLoading.ShowMsg("تعذر الاتصال بالخادم", "حاول مره أخري");
                        return;
                    }
                    else
                    {
                        var temp = JsonConvert.DeserializeObject<SignUpCode>(rv);

                        if (temp.Used != 0)
                        {
                            _PopUpLoading.ShowMsg("هذا الكود تم أستخدامة سابقا", "موافق");
                            return;
                        }
                        else
                        {
                            _PopUpLoading.Done();

                            if (can)
                            {
                                SignupPage.SUC = temp;
                                //await Consts.navto(Routes.SignupPage);

                                await AppShell.GoToPage(nameof(SignupPage));
                            }
                            else
                                await MsgWithIcon.ShowError("يرجى الموافقة على الأذونات للمتابعة.", Navigation, "موافق");

                            //await PopupNavigation.Instance.PushAsync(new Msg("يرجى الموافقة على الأذونات للمتابعة.", "خطأ:", Color.FromArgb("#ff3b2f"), "موافق"));
                        }
                    }
                }
            }
            else
            {
                if (await ConfirmMsg.ConfirmMessage("يرجي الموافقة على الأذونات للمتابعة.", "تأكيد:", "ليس الأن", "الإعدادت", Navigation))
                {
                    AppInfo.ShowSettingsUI();
                }
            }

        }, Navigation);

    }

}