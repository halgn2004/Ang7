using Ang7.ViewModels;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AL_TeacherFilesManger : ContentPage
{
    AL_TeacherFilesMangerViewModel vm;
    public AL_TeacherFilesManger()
    {
        InitializeComponent();
        BindingContext = vm = new AL_TeacherFilesMangerViewModel(Navigation);

/*#if IOS
        ScreenSecurity.Default.DeactivateScreenSecurityProtection();
#endif*/

    }
/*#if IOS
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ScreenSecurity.Default.ActivateScreenSecurityProtection();
    }
#endif*/

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
        if (vm.IsUpAvalible)
            {
                vm.UpClicked.Execute(false);
            }
            else
                Navigation.PopAsync();
        //});
        return true;
    }

}