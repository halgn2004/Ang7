using Ang7.Models;
using Ang7.ViewModels;
using RGPopup.Maui.Services;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
//[QueryProperty(nameof(TeacherJson), "teacherJson")]
public partial class AL_TeacherView : ContentPage
{
/*    public string TeacherJson
    {
        set
        {
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                Teacher = JsonConvert.DeserializeObject<Teacher>(Uri.UnescapeDataString(value.ToString()));
                BindingContext = vm = new AL_TeacherViewModel(Teacher, Navigation);
                // Use the Teacher object here
                //Console.WriteLine($"Name: {Teacher.Name}, Email: {Teacher.Email}");
            }
        }
    }

    public Teacher Teacher { get; set; }*/

    AL_TeacherViewModel vm;
    public AL_TeacherView(Teacher teacher)
    {
        InitializeComponent();
        BindingContext = vm = new AL_TeacherViewModel(teacher, Navigation);
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
        if (vm.IsUpAvalible)
            {
                vm.UpClicked.Execute(false);
            }
            else
                Navigation.PopAsync();
        //});
        return true;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Unsubscribe event handlers or cancel any tasks running in the background
    }
}