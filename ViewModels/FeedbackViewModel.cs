using Ang7.Views;
using RGPopup.Maui.Services;

namespace Ang7.ViewModels;

public class FeedbackViewModel : BaseViewModel
{
    public FeedbackViewModel()
    {
        SendEmailCommand = new Command(SendEmailAction);
    }
    public FeedbackViewModel(bool shkwa)
    {
        SendEmailCommand = new Command(SendEmailAction);
        PickerSelectedIndex = 3;
    }

    private string _note;
    public string Note
    {
        get
        {
            return _note;
        }
        set
        {
            if (SetProperty(ref _note, value))
            {
                IsEn = CheckEn();
            }
        }
    }

    public Command SendEmailCommand { get; }


    private bool _isShowmsg;
    public bool IsShowmsg
    {
        get { return _isShowmsg; }
        set { SetProperty(ref _isShowmsg, value); }
    }


    private bool _Activated = false;
    public bool Activated
    {
        get { return _Activated; }
        set { SetProperty(ref _Activated, value); }
    }
    private string _msg;
    public string Msg
    {
        get { return _msg; }
        set { SetProperty(ref _msg, value); }
    }
    private string _ButSEImage = "sendfeedbackdis.png";
    public string ButSEImage
    {
        get { return _ButSEImage; }
        set { SetProperty(ref _ButSEImage, value); }
    }
    private bool _IsEn;
    public bool IsEn
    {
        get
        {
            return _IsEn;
        }
        set
        {
            SetProperty(ref _IsEn, value);
            ButSEImage = (value) ? "sendfeedback.png" : "sendfeedbackdis.png";
        }
    }
    private int _PickerSelectedIndex = -1;
    public int PickerSelectedIndex
    {
        get { return _PickerSelectedIndex; }
        set
        {
            if (SetProperty(ref _PickerSelectedIndex, value))
            {
                IsEn = CheckEn();
            }
        }
    }
    private bool CheckEn()
    {
        if (PickerSelectedIndex != -1 && Note != null && Note.Length > 2)
            return true;
        else
            return false;
    }

    private void SendEmailAction(object obj)
    {
        ButSEImage = "sendfeedbackfocus.png";
        Task.Delay(50);
        ButSEImage = "sendfeedback.png";
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            ViewMsg("No Internet Access", false);
        }
        else
        {
            
            Task.Factory.StartNew(() => {
                IsEn = false;
                ViewMsg("جاري الإرسال", true);
                string rv = GlobalFunc.APISendFeedBack(AL_HomePage.CU.UserID,PickerSelectedIndex,Note).Result;
                if (rv == "Done.")
                {
                    ViewMsg("تم الإرسال بنجاح", false);
                    Note = string.Empty;
                }
                else
                {
                    ViewMsg("فشل الإرسال", false);
                    IsEn = true;
                }
            }).ContinueWith((t) => {
                Task.Delay(1500).ContinueWith((t2) => {
                    IsShowmsg = false;
                    PopupNavigation.Instance.PopAsync();
                });
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
    void ViewMsg(string msg, bool IsActivityInductor)
    {
        IsShowmsg = true;
        Msg = msg;
        Activated = IsActivityInductor;
    }
}