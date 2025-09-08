using Ang7.ViewModels;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[QueryProperty(nameof(Email), "email")]
[QueryProperty(nameof(Password), "password")]
[QueryProperty(nameof(From), "from")]
public partial class SigninPage : ContentPage
{
    public string Email
    {
        set
        {
            vm.Set_Email(value.ToString());
        }
    }
    public string Password
    {
        set
        {
            vm.Set_Password(value.ToString());
        }
    }
    public string From { get; set; }

    SigninPageViewModel vm = null;

    public SigninPage()
    {
        InitializeComponent();
        BindingContext = vm = new SigninPageViewModel(Navigation);
        Helpers.Settings.KeepMeLoggedIn_SetUser(null);
        Helpers.Settings.GeneralSettings_KeepMeLoggedIn = false;
    }
    /*public SigninPage(string na, string pw)
    {
        InitializeComponent();
        BindingContext = vm = new SigninPageViewModel(Navigation, na, pw);
    }*/
    protected override bool OnBackButtonPressed()
    {
        // Check if there's a popup being displayed
        if (PopupNavigation.Instance.PopupStack.Any())
        {
            // Close the current popup

            TapEventHandler.Reset_isNavigating();
            PopupNavigation.Instance.PopAsync();
            return true; // Return true to prevent the back press from affecting the main page navigation
        }
        // If no popups are displayed, return false to allow back navigation on the main page
        return false;
    }
    async void Onback_Clicked(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(false, async () =>
        {
            if (string.IsNullOrEmpty(From))
            {
                await AppShell.GoToPage(nameof(HomePage));
            }
            else
            {
                if(From == nameof(SignupPage) && SignupPage.SUC != null && SignupPage.SUC.Used == 0)
                {
                    var parameters = new ShellNavigationQueryParameters();
                    if (!string.IsNullOrEmpty(vm.Email))
                        parameters.Add("email", vm.Email);
                    if (!string.IsNullOrEmpty(vm.Password))
                        parameters.Add("password", vm.Password);

                    await AppShell.GoToPage(nameof(SignupPage), parameters);
                }
                else
                {
                    await AppShell.GoToPage(nameof(HomePage));
                }
            }
            /*if (SignupPage.SUC != null && SignupPage.SUC.Used == 1)
            {
                SignupPage.SUC = null;
                AppShell.GoToPage(nameof(HomePage));
                //await Consts.navto(Routes.HomePage, clearStack: true);
            }
            else
                AppShell.GoToPage("..");
            //await Consts.navto("..");
            //await Navigation.PopAsync();*/
        }, Navigation);
    }
}