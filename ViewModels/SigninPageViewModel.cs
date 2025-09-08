using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class SigninPageViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    PopUpLoadingViewModel _PopUpLoading;

    public static User CU = null;

    public SigninPageViewModel(INavigation _navigation)
    {
        Navigation = _navigation;
        CU = null;
    }
    /*public SigninPageViewModel(INavigation _navigation, string s1, string s2)
    {
        Navigation = _navigation;
        Email = s1;
        Password = s2;
    }*/
    /*public void SetCredentials(string email, string password)
    {
        Email = email;
        Password = password;

        // Do something with the credentials (e.g., login logic)
        //Console.WriteLine($"Email: {_email}, Password: {_password}");
    }*/
    public void Set_Email(string email)
    {
        Email = email;
    }
    public void Set_Password(string password)
    {
        Password = password;
    }
    #region Properties


    private string _email;
    public string Email
    {
        get { return _email; }
        set
        {
            if (SetProperty(ref _email, value))
            {
                CanLoginAction();
            }
        }
    }

    private string _LoginButText = "تسجيل دخول";
    public string LoginButText
    {
        get { return _LoginButText; }
        set { SetProperty(ref _LoginButText, value); }
    }
    private bool _IsLoginButEnable;
    public bool IsLoginButEnable
    {
        get { return _IsLoginButEnable; }
        set { SetProperty(ref _IsLoginButEnable, value); }
    }

    private string _password;
    public string Password
    {
        get { return _password; }
        set
        {
            if (SetProperty(ref _password, value))
            {
                CanLoginAction();
            }
        }
    }
    private string _VC;
    public string VC
    {
        get { return _VC; }
        set
        {
            if (SetProperty(ref _VC, value))
            {
                CanLoginAction();
            }
        }
    }

    private string _resend = "أرسال مرة أخري";
    public string Resend
    {
        get { return _resend; }
        set
        {
            if (SetProperty(ref _resend, value))
            {
                ((Command)ResendCommand).ChangeCanExecute();
            }
        }
    }
    private bool _Activated = false;
    public bool Activated
    {
        get { return _Activated; }
        set { SetProperty(ref _Activated, value); }
    }
    private bool _CheckBoxStatus = Helpers.Settings.GeneralSettings_KeepMeLoggedIn;
    public bool CheckBoxStatus
    {
        get { return _CheckBoxStatus; }
        set { SetProperty(ref _CheckBoxStatus, value); }
    }
    private bool _EmailValidationMsg;
    public bool EmailValidationMsg
    {
        get { return _EmailValidationMsg; }
        set { SetProperty(ref _EmailValidationMsg, value); }
    }
    private bool _PWValidationMsg;
    public bool PWValidationMsg
    {
        get { return _PWValidationMsg; }
        set { SetProperty(ref _PWValidationMsg, value); }
    }

    #endregion


    #region Commands

    private ICommand _loginCommand;
    public ICommand LoginCommand
    {
        get { return _loginCommand = _loginCommand ?? new Command(LoginAction); }
    }

    private ICommand _ResendCommand;
    public ICommand ResendCommand
    {
        get { return _ResendCommand = _ResendCommand ?? new Command(ResendAction, CanResend); }
    }


    private ICommand _forgotPasswordCommand;
    public ICommand ForgotPasswordCommand
    {
        get { return _forgotPasswordCommand = _forgotPasswordCommand ?? new Command(ForgotPasswordAction); }
    }
    private ICommand _CheckBoxStatusLabelclickedCommand;
    public ICommand CheckBoxStatusLabelclickedCommand
    {
        get { return _CheckBoxStatusLabelclickedCommand = _CheckBoxStatusLabelclickedCommand ?? new Command(CheckBoxStatusLabelclickedAction); }
    }

    #endregion


    #region Methods

    void CanLoginAction()
    {
        if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.Password))
        {
            IsLoginButEnable = false;
            return;
        }
        if (!GlobalFunc.IsEmail(Email) || Password.Length < 6 || CU != null)
        {
            IsLoginButEnable = false;
            return;
        }
        if (LoginButText == "تفعيل وتسجيل دخول")
        {
            if (string.IsNullOrWhiteSpace(this.VC))
            {
                IsLoginButEnable = false;
                return;
            }
            if (VC.Length < 6)
            {
                IsLoginButEnable = false;
                return;
            }
        }
        IsLoginButEnable = true;
    }
    User temp;
    //ObservableCollection<NotesByRoom> ntemp;
    async void LoginAction()
    {
        _PopUpLoading = new PopUpLoadingViewModel(Navigation);
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var can = await GlobalFunc.CheckPermissionsAndStart();
            if (can)
            {
                await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));
                getApiResponse();
            }
            else
            {
                if(await ConfirmMsg.ConfirmMessage("يرجي الموافقة على الأذونات للمتابعة.", "تأكيد:", "ليس الأن", "الإعدادت", Navigation))
                {
                    AppInfo.ShowSettingsUI();
                }
            }
        }, Navigation);
    }
    public async void getApiResponse()
    {
        string rv2 = await GlobalFunc.TestGettoken(this.Email, this.Password);
        if (rv2 == "ErrorToken")
        {
            _PopUpLoading.ShowMsg("برجاءالدخول من نفس الجهاز المفعل عليه الحساب", "موافق");
            return;
        }

        string rv = await GlobalFunc.isuser(this.Email, this.Password, VC);
        //_PopUpLoading.ShowMsg($"RV:{rv}\n\nEmail:{Email}\nPW:{Password}\nVC:{VC}", "test");
        //return;
        if (rv == "error01")
        {
            _PopUpLoading.ShowMsg("خطأ في اسم المستخدم أو كلمة مرور", "حاول مره أخري");
            Activated = false;
            return;
        }
        else if (rv == "error02")
        {
            if (await ConfirmMsg.ConfirmMessage("هل تود آستعادت حسابك ؟", "تأكيد:", "لا", "نعم", Navigation))
            {
                var json = await GlobalFunc.APIRecoverAcc(this.Email, this.Password);
                if (json == "Done.")
                {
                    _PopUpLoading.ShowMsg("لقد تم آستعادت هذا الحساب بنجاح، برجاء تسجيل الدخول مجددا.", "موافق");
                    return;
                }
            }
            _PopUpLoading.ShowMsg("لقد تم حذف هذا الحساب مؤقتا برجاء التواصل معنا لأسترداد الحساب قبل مرور ۳۰ يوم منذ حذفه لتجنب فقدان بياناتك.", "موافق");
            return;
        }
        else if (rv == "erorr404")
        {
            _PopUpLoading.ShowMsg("تعذر الاتصال بالخادم", "حاول مره أخري");
            return;
        }
        else
        {
            temp = JsonConvert.DeserializeObject<User>(rv);
            //string s = "";

            if (temp.Activated == 0)
            {
                Activated = true;
                if (LoginButText == "تسجيل دخول")
                {
                    _PopUpLoading.Done();
                    LoginButText = "تفعيل وتسجيل دخول";
                    CanLoginAction();
                    _ = await GlobalFunc.genvc(temp.UserID.ToString(), 2);
                    return;
                }
                if (await GlobalFunc.checkvc(temp.UserID.ToString(), VC, 2) == "succ2")
                {
                    CU = temp;
                    CU.Activated = 1;
                    LoginButText = "تسجيل دخول";
                }
                else
                {
                    _PopUpLoading.ShowMsg("رمز التحقق غير صالح", "حاول مره أخري");
                }
            }
            else
            {
                Activated = false;
                LoginButText = "تسجيل دخول";
                CU = temp;
            }
        }
        if (CU != null)
        {
            AL_HomePage.CU = CU;
            if (CheckBoxStatus)
            {
                Helpers.Settings.KeepMeLoggedIn_SetUser(CU);
            }
            _PopUpLoading.Done();
            //await Navigation.PushAsync(new AL_HomePage());

            await AppShell.GoToPage(nameof(AL_HomePage));
            //await Consts.navto(nameof(AL_HomePage), clearStack: true);
        }
    }

    private void CheckBoxStatusLabelclickedAction(object obj)
    {
        CheckBoxStatus = !CheckBoxStatus;
    }

    private async void ResendAction(object obj)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var res = await ConfirmMsg.ConfirmMessage("لن يكون عليك استخدام الرمز القديم بعد الآن\nهل أنت متأكد من إعادة إرسال رمز جديد لك؟", "تاكيد:", "إلغاء", "موافق", Navigation);
            if (res)
            {
                Resend = "60";
                int seconds = 60;
                // TODO Xamarin.Forms.Device.StartTimer is no longer supported. Use Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                Application.Current?.Dispatcher?.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    seconds--;
                    if (seconds > 9)
                        Resend = seconds.ToString();
                    else
                        Resend = "0" + seconds.ToString();

                    if (seconds == 0)
                    {
                        Resend = "أرسال مرة أخري";
                        return false; // Stops the timer
                    }

                    return true; // Keeps the timer running
                });
                await Task.Delay(100).ContinueWith((t) => GlobalFunc.genvc(temp.UserID.ToString(), 2));
            }
        }, Navigation);
    }
    private bool CanResend(object arg)
    {
        return (Resend == "أرسال مرة أخري");
    }


    PopUpForgotPasswordViewModel fpvm = null;
    async void ForgotPasswordAction()
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            if (fpvm == null)
            {
                fpvm = new PopUpForgotPasswordViewModel(this.Email, Navigation);
            }
            else if (string.IsNullOrEmpty(fpvm.Email) || fpvm.Email != this.Email)
            {
                if (fpvm.Resend == "أرسال مرة أخري" || fpvm.Resend == "إرسال الرمز")
                    fpvm = new PopUpForgotPasswordViewModel(this.Email, Navigation);
            }
            await PopupNavigation.Instance.PushAsync(new ForgotPassword(fpvm));
        }, Navigation);
    }
    #endregion
}
