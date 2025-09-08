using Ang7.Views.PopUp;
using Plugin.Maui.AppRating;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AL_Settings : ContentPage
{
    int DefFontSize = 18, UpdatedFS = 13;
    private readonly IAppRating _appRating;
    public AL_Settings()
    {
        InitializeComponent();
        VersionLabel.Text = Consts.AppVersion;
        _appRating = DependencyService.Get<IAppRating>();

        SC_SS.IsToggled = Helpers.Settings.GeneralSettings_SkipSS;
        SC_Intro.IsToggled = Helpers.Settings.GeneralSettings_SkipIntro;
        if(AL_HomePage.CU == null || AL_HomePage.CU.UserType == -1)
        {
            SC_KeepMeLoggedInFrame.IsEnabled = false;
            SC_KeepMeLoggedIn.IsToggled = false;
            Helpers.Settings.GeneralSettings_KeepMeLoggedIn = false;
        }
        else
        {
            SC_KeepMeLoggedIn.IsToggled = Helpers.Settings.GeneralSettings_KeepMeLoggedIn;
        }

        if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
        {
            DefFontSize = 36;
            UpdatedFS = 32;
        }
    }
    private void SwitchatFun(int switchID)
    {
        switch (switchID)
        {
            case 1:
                SC_SSFrame.Stroke = Color.FromArgb("#0AB0B0");
                _ = Task.Delay(200).ContinueWith((t) => SC_SSFrame.Stroke = Color.FromArgb("#f6f8f9"));
                Helpers.Settings.GeneralSettings_SkipSS = SC_SS.IsToggled;
                //GlobalFunc.ToastShow($"Setting :{Helpers.Settings.GeneralSettings_SkipSS.ToString()}  Switch:{SC_SS.IsToggled.ToString()}");
                break;
            case 2:
                SC_IntroFrame.Stroke = Color.FromArgb("#0AB0B0");
                _ = Task.Delay(200).ContinueWith((t) => SC_IntroFrame.Stroke = Color.FromArgb("#f6f8f9"));
                Helpers.Settings.GeneralSettings_SkipIntro = SC_Intro.IsToggled;
                //GlobalFunc.ToastShow($"Setting :{Helpers.Settings.GeneralSettings_SkipIntro.ToString()}  Switch:{SC_Intro.IsToggled.ToString()}");
                break;
            case 3:
                SC_KeepMeLoggedInFrame.Stroke = Color.FromArgb("#0AB0B0");
                _ = Task.Delay(200).ContinueWith((t) => SC_KeepMeLoggedInFrame.Stroke = Color.FromArgb("#f6f8f9"));
                Helpers.Settings.GeneralSettings_KeepMeLoggedIn = SC_KeepMeLoggedIn.IsToggled;
                if (SC_KeepMeLoggedIn.IsToggled)
                {
                    if(Helpers.Settings.KeepMeLoggedIn_GetUser() == null)
                        Helpers.Settings.KeepMeLoggedIn_SetUser(AL_HomePage.CU);
                }
                //GlobalFunc.ToastShow($"Setting :{Helpers.Settings.GeneralSettings_KeepMeLoggedIn.ToString()}  Switch:{SC_KeepMeLoggedIn.IsToggled.ToString()}");
                break;
        }
    }
    private void Switch_SC_SS(object sender, ToggledEventArgs e)
    {
        SwitchatFun(1);
       
        /*var s = sender as Switch;
        s.IsToggled = !s.IsToggled;*/
    }
    private void OnSC_SS(object sender, EventArgs e)
    {
        //SwitchatFun(1);
        SC_SS.IsToggled = !SC_SS.IsToggled;
    }
    private void Switch_SC_Intro(object sender, ToggledEventArgs e)
    {
        SwitchatFun(2);
    }
    private void OnSC_Intro(object sender, EventArgs e)
    {
        //SwitchatFun(2);
        SC_Intro.IsToggled = !SC_Intro.IsToggled;
    }
    private void Switch_SC_KeepMeLoggedIn(object sender, ToggledEventArgs e)
    {
        SwitchatFun(3);
    }
    private void OnSC_KeepMeLoggedIn(object sender, EventArgs e)
    {
        //SwitchatFun(3);
        SC_KeepMeLoggedIn.IsToggled = !SC_KeepMeLoggedIn.IsToggled;
    }
    private async void OnSignOut(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(false, async () =>
        {
            SOFrame.Stroke = Colors.Red;
            //await Task.Delay(200);
            if (await ConfirmMsg.ConfirmMessage("هل أنت متأكد أنك تود تسجيل الخروج ؟", "تأكيد:", "لا", "نعم", Navigation))
            {
                AL_HomePage.CU = null;
                await AppShell.GoToPage(nameof(HomePage));
                //await Navigation.PushAsync(new HomePage());
            }
            SOFrame.Stroke = Color.FromArgb("#f6f8f9");
        }, Navigation);
    }
    private void Onback_Clicked(object sender, EventArgs e)
    {
        if (TitleLabel.Text == "الإعدادات")
        {
            Navigation.PopAsync();
        }
        else
        {
            TitleLabel.Text = "الإعدادات";
            TitleLabel.FontSize = DefFontSize;
            WV.IsVisible = false;
            SettingsTable.IsVisible = true;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (PopupNavigation.Instance.PopupStack.Any())
        {
            TapEventHandler.Reset_isNavigating();
            PopupNavigation.Instance.PopAsync();
            return true; // Return true to prevent the back press from affecting the main page navigation
        }

        if (TitleLabel.Text == "الإعدادات")
        {
            Navigation.PopAsync();
        }
        else
        {
            TitleLabel.Text = "الإعدادات";
            TitleLabel.FontSize = DefFontSize;
            WV.IsVisible = false;
            SettingsTable.IsVisible = true;
        }
        return true;
    }
    private async void OnRateTapped(object sender, EventArgs e)
    {
        RateFrame.Stroke = Color.FromArgb("#0AB0B0");
        _ = Task.Delay(200).ContinueWith((t) => RateFrame.Stroke = Color.FromArgb("#f6f8f9"));
        if (!Preferences.Get("application_rated", false))
        {
            await TapEventHandler.HandleTapEvent(true, async () =>
            {
                await Task.Run(RateApplicationOnStore);
            }, Navigation);
        }
    }

    private async void RateApplicationOnStore()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
           if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Launcher.OpenAsync($"market://details?id={Consts.androidPackageName}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Launcher.OpenAsync($"itms-apps://itunes.apple.com/app/{Consts.iOSApplicationId}?action=write-review");
            }
        });
    }

    private async void OnPPTapped(object sender, EventArgs e)
    {
        PPFrame.Stroke = Color.FromArgb("#0AB0B0");
        WV.Source = new HtmlWebViewSource
        {
            Html = Consts.PrivacyPolicy_HTML
        };
        await Task.Delay(200);
        PPFrame.Stroke = Color.FromArgb("#f6f8f9");
        TitleLabel.Text = "الإعدادات > سياسة الخصوصية";
        TitleLabel.FontSize = UpdatedFS;
        SettingsTable.IsVisible = false;
        WV.IsVisible = true;
    }
    private async void OnTSTapped(object sender, EventArgs e)
    {
        TSFrame.Stroke = Color.FromArgb("#0AB0B0");
        WV.Source = new HtmlWebViewSource
        {
            Html = Consts.TermsOfService_HTML
        };
        await Task.Delay(200);
        TSFrame.Stroke = Color.FromArgb("#f6f8f9");
        TitleLabel.Text = "الإعدادات > شروط الخدمة";
        TitleLabel.FontSize = UpdatedFS - 2;
        SettingsTable.IsVisible = false;
        WV.IsVisible = true;
    }
    private async void OnVersionTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            VersionFrame.Stroke = Color.FromArgb("#0AB0B0");
            if (!VersionAI.IsVisible)
            {
                VersionAI.IsVisible = true;
                VersionAI.IsRunning = true;
                //VersionViewCell.IsEnabled = false;
                try
                {
                    var NeedUpdate = await Validate.CheckForUpdate();
                    if (NeedUpdate)
                    {
                        await GlobalFunc.RedirectToAppStore();
                    }
                    else
                    {
                        await MsgWithIcon.ShowUpToDate(Navigation);
                    }
                }
                catch
                {
                    await MsgWithIcon.ShowUpToDate(Navigation);
                }
                VersionAI.IsVisible = false;
                VersionAI.IsRunning = false;
                //VersionViewCell.IsEnabled = true;
            }
            VersionFrame.Stroke = Color.FromArgb("#f6f8f9");
        }, Navigation);
    }
}