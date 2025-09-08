using Ang7.Views.PopUp;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class PopUpForgotPasswordViewModel : BaseViewModel
{
    private readonly INavigation Navigation;

    public PopUpForgotPasswordViewModel(string n, INavigation _navigation)
    {
        Email = n;
        Navigation = _navigation;
    }
    public PopUpForgotPasswordViewModel(INavigation _navigation)
    {
        Navigation = _navigation;
    }


    private string _Email;
    public string Email
    {
        get { return _Email; }
        set
        {
            if (SetProperty(ref _Email, value))
            {
                ((Command)SubmitCommand).ChangeCanExecute();
                ((Command)ResendCommand).ChangeCanExecute();
            }
        }
    }

    private string _vc;

    public string VC
    {
        get
        {
            return _vc;
        }
        set
        {
            if (SetProperty(ref _vc, value))
            {
                ((Command)SubmitCommand).ChangeCanExecute();
            }
        }
    }

    private string _msg;

    public string Msg
    {
        get
        {
            return _msg;
        }
        set
        {
            SetProperty(ref _msg, value);
        }
    }

    private bool _msga;

    public bool Msga
    {
        get
        {
            return _msga;
        }
        set
        {
            SetProperty(ref _msga, value);
        }
    }
    private string _resend = "إرسال الرمز";
    public string Resend
    {
        get { return _resend; }
        set
        {
            if (SetProperty(ref _resend, value))
            {
                OnPropertyChanged(nameof(Resendfontsize));
                ((Command)ResendCommand).ChangeCanExecute();
            }
        }
    }
    //private int _resendfontsize = 8;
    public int Resendfontsize => (Resend.Length <= 2) ? 10 : 7;
    /*{
        get { return _resendfontsize; }
        set
        {
            if (SetProperty(ref _resendfontsize, value))
            {
                ((Command)ResendCommand).ChangeCanExecute();
            }
        }
    }*/

    private ICommand _submitCommand;
    public ICommand SubmitCommand
    {
        get { return _submitCommand = _submitCommand ?? new Command(SubmitAction, CanSubmitAction); }
    }
    private ICommand _ResendCommand;
    public ICommand ResendCommand
    {
        get { return _ResendCommand = _ResendCommand ?? new Command(ResendAction, CanResend); }
    }

    private async void SubmitAction(object obj)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await MsgWithIcon.ShowNoConn(Navigation);
            return;
        }
        else
        {
            await Task.Factory.StartNew(() =>
            {
                if (GlobalFunc.checkvc(Email, VC, 1).Result == "succ1")
                {
                    ShowMsg("لقد تم إرسال كلمة المرور الجديدة إلى بريدك الإلكتروني.");
                }
                else
                {
                    ShowMsg("رمز التحقق غير صالح.");
                }
            });
        }
    }

    private bool CanSubmitAction(object arg)
    {
        if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.VC))
        {
            return false;
        }
        return VC.Length == 6 && GlobalFunc.IsEmail(Email);
    }

    private async void ResendAction(object obj)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await MsgWithIcon.ShowNoConn(Navigation);
            return;
        }
        bool res = true;
        if (Resend == "أرسال مرة أخري")
            res = await ConfirmMsg.ConfirmMessage("لن يمكن استخدام الرمز القديم بعد الآن\nهل أنت متأكد من إعادة إرسال رمز جديد لك؟", "تاكيد:", "إلغاء", "موافق", Navigation);
        else
        {
            if (res)
            {
                Resend = "60";
                int seconds = 60;
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



                //await Task.Delay(100).ContinueWith((t) => ShowMsg(GlobalFunc.genvc(Email, 1).Result));
                await Task.Delay(100).ContinueWith((t) => GlobalFunc.genvc(Email, 1).Result);
            }
        }
    }
    private bool CanResend(object arg)
    {
        if (string.IsNullOrWhiteSpace(this.Email) || !GlobalFunc.IsEmail(Email))
        {
            return false;
        }
        return (Resend == "أرسال مرة أخري" || Resend == "إرسال الرمز");
    }

    private void ShowMsg(string M)
    {
        Msg = M;
        Msga = true;
    }
}