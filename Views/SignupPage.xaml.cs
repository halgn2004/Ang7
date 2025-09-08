using Ang7.Models;
using Ang7.ViewModels;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[QueryProperty(nameof(Email), "email")]
[QueryProperty(nameof(Password), "password")]
public partial class SignupPage : ContentPage
{
    public static SignUpCode SUC { get; set; }
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
    SignupPageViewModel vm;
    public SignupPage()
    {
        InitializeComponent();

        BindingContext = vm = new SignupPageViewModel(Navigation);

        //_ = MsgWithIcon.ShowError("3", Navigation, "موافق");

        //BindingContext = vm = new SignupPageViewModel(signUpCode, Navigation);
    }
    async void Onback_Clicked(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(false, async () =>
        {
            //await Navigation.PopAsync();
            //await Shell.Current.GoToAsync("..");
            //await Consts.navto("..");

            await AppShell.GoToPage(nameof(HomePage));

        }, Navigation);
    }
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        /*if (!string.IsNullOrEmpty(Email))
            vm.Set_Email(Email);
        if (!string.IsNullOrEmpty(Password))
            vm.Set_Password(Password);*/
    }
}