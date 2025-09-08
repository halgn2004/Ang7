using Ang7.Views.PopUp;
using Plugin.Maui.AppRating;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
//[QueryProperty(nameof(AppRating), "_appRating")]
public partial class SettingsPage : ContentPage
{
    int DefFontSize = 18, UpdatedFS = 13;
    private readonly IAppRating _appRating;
    public SettingsPage(/*IAppRating appRating*/)
    {
        InitializeComponent();
        VersionLabel.Text = Consts.AppVersion;
        // Resolve the dependency via DI or DependencyService
        _appRating = DependencyService.Get<IAppRating>();
        if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
        {
            DefFontSize = 36;
            UpdatedFS = 32;
        }
    }
    private async void RateApplicationOnStore()
    {
        /*await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                await _appRating.PerformRatingOnStoreAsync(packageName: Consts.androidPackageName, applicationId: Consts.iOSApplicationId, productId: Consts.windowsProductId);
                Preferences.Set("application_rated", true);
            }
            catch
            {
                //
            }
        });

        //return Task.CompletedTask;*/
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
    private void Onback_Clicked(object sender, EventArgs e)
    {
        if (TitleLabel.Text == "الإعدادات")
        {
            Navigation.PopAsync();
            //_ = Consts.navto("..");

            //AppShell.GoToPage("..");
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
            return true;
        }
        if (TitleLabel.Text == "الإعدادات")
        {
            Navigation.PopAsync();
            //_ = Consts.navto("..");
            //AppShell.GoToPage("..");
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
            },Navigation);

        }
        //await CrossStoreReview.Current.RequestReview(false);
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
                //VersionFrame.IsEnabled = false;
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
                        //await PopupNavigation.Instance.PushAsync(new Msg("التطبيق أحدث إصدار", null, Colors.Black, "موافق"));
                    }
                }
                catch
                {
                    await MsgWithIcon.ShowUpToDate(Navigation);
                    //await PopupNavigation.Instance.PushAsync(new Msg("التطبيق أحدث إصدار.", null, Colors.Black, "موافق"));
                }
                VersionAI.IsVisible = false;
                VersionAI.IsRunning = false;
                //VersionFrame.IsEnabled = true;
            }
            VersionFrame.Stroke = Color.FromArgb("#f6f8f9");
        }, Navigation);

    }

}