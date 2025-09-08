using System.Windows.Input;
//using Plugin.DeviceInfo;
//using Acr.UserDialogs;
using Ang7.Views;
using Ang7.Views.PopUp;
using RGPopup.Maui.Services;

namespace Ang7.ViewModels;

public class PopUpChangePWViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    public PopUpChangePWViewModel(INavigation _navigation)
    {
        Navigation = _navigation;
    }


    private string _OldPassword;
    public string OldPassword
    {
        get { return _OldPassword; }
        set
        {
            if (SetProperty(ref _OldPassword, value))
            {
                ((Command)ChangePWCommand).ChangeCanExecute();
            }
        }
    }
    private string _NewPassword;
    public string NewPassword
    {
        get { return _NewPassword; }
        set
        {
            if (SetProperty(ref _NewPassword, value))
            {
                ((Command)ChangePWCommand).ChangeCanExecute();
                PWCheck();
            }
        }
    }
    private string _NewPassword2;
    public string NewPassword2
    {
        get { return _NewPassword2; }
        set
        {
            if (SetProperty(ref _NewPassword2, value))
            {
                ((Command)ChangePWCommand).ChangeCanExecute();
                PWCheck();
            }
        }
    }
    private bool _PWCValidationMsg;
    public bool PWCValidationMsg
    {
        get { return _PWCValidationMsg; }
        set { SetProperty(ref _PWCValidationMsg, value); }
    }
    public void PWCheck()
    {
        if (string.IsNullOrWhiteSpace(this.NewPassword2) || string.IsNullOrWhiteSpace(this.NewPassword2))
            PWCValidationMsg = false;
        else if (!string.Equals(NewPassword, NewPassword2, StringComparison.Ordinal))
            PWCValidationMsg = true;
        else
            PWCValidationMsg = false;
        OnPropertyChanged(nameof(PWCValidationMsg));
    }

    /*private bool _IsShowPassword;
    public bool IsShowPassword
    {
        get { return _IsShowPassword; }
        set { SetProperty(ref _IsShowPassword, value); }
    }
    private bool _IsShowPassword2;
    public bool IsShowPassword2
    {
        get { return _IsShowPassword2; }
        set { SetProperty(ref _IsShowPassword2, value); }
    }*/


    private ICommand _ChangePWCommand;
    public ICommand ChangePWCommand
    {
        get { return _ChangePWCommand = _ChangePWCommand ?? new Command(ChangePWAction, CanChangePWAction); }
    }

    /*private ICommand _ShowPasswordCommand;
    public ICommand ShowPasswordCommand
    {
        get { return _ShowPasswordCommand = _ShowPasswordCommand ?? new Command(ShowPasswordAction); }
    }
    private ICommand _ShowPasswordCommand2;
    public ICommand ShowPasswordCommand2
    {
        get { return _ShowPasswordCommand2 = _ShowPasswordCommand2 ?? new Command(ShowPassword2Action); }
    }*/

    private async void ChangePWAction(object obj)
    {
        try
        {
            var s = await UpdatePW(NewPassword, OldPassword);
            if (s == "Wrong Password")
            {
                await PopupNavigation.Instance.PushAsync(new Msg("كلمة مرور خاطئة", "خطأ", Color.FromArgb("#ff3b2f"), "موافق"));
                //await App.Current.MainPage.DisplayAlert("خطأ", "كلمة مرور خاطئة", "موافق");
                //UserDialogs.Instance.Toast($"Wrong Password");
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new Msg("تم تغيير كلمة المرور بنجاح", "تجاح", Color.FromArgb("#ff3b2f"), "موافق"));
                //await App.Current.MainPage.DisplayAlert("نجاح", "تم تغيير كلمة المرور بنجاح", "موافق");
                //UserDialogs.Instance.Toast($"Successfully changed your Password");
            }
        }
        catch
        {
            await PopupNavigation.Instance.PushAsync(new Msg("لم يتمكن من الوصول الي الخادم", "خطأ", Color.FromArgb("#ff3b2f"), "موافق"));
            //await App.Current.MainPage.DisplayAlert("خطأ", "لم يتمكن من الوصول الي الخادم", "موافق");
            //UserDialogs.Instance.Toast($"Error : Couldn't Connected to the server.");
            return;
        }
        await PopupNavigation.Instance.PopAllAsync();

    }
    private bool CanChangePWAction(object arg)
    {
        if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword2))
            return false;
        
        return NewPassword == NewPassword2 && OldPassword != NewPassword && NewPassword.Length > 5 && Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
    /*private void ShowPasswordAction(object obj)
    {
        IsShowPassword = !IsShowPassword;
    }
    private void ShowPassword2Action(object obj)
    {
        IsShowPassword2 = !IsShowPassword2;
    }*/

    private async Task<string> UpdatePW(string newpw, string oldpw)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(AL_HomePage.CU.UserID.ToString()), "\"uid\"" },
            { new StringContent("4"), "\"t\"" },
            { new StringContent(newpw), "\"v\"" },
            { new StringContent(oldpw), "\"opw\"" }
        };
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync($"{Consts.APIUrl}UpdateUser.php", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

}