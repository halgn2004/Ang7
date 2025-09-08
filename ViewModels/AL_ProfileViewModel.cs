using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using RGPopup.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class AL_ProfileViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    private User _user;
    public User User
    {
        get { return _user; }
        set
        {
            /*if (SetProperty(ref _user, value))
            {
                OnPropertyChanged(nameof(BIOHigh));
                //Subjects = GlobalFunc.GetSubjectsList(AL_HomePage.CU.Subjects);
            }*/
            _user = value;
            OnPropertyChanged();
            if (AL_HomePage.CU.Subjects != null) {
                OnPropertyChanged(nameof(BIOHigh));
                Subjects = GlobalFunc.GetSubjectsList(AL_HomePage.CU.Subjects);
            }
        }
    }
    private ObservableCollection<Subject> _Subjects = new ObservableCollection<Subject>();
    public ObservableCollection<Subject> Subjects
    {
        get { return _Subjects; }
        set {
            if (SetProperty(ref _Subjects, value))
            {
                OnPropertyChanged(nameof(SubjHigh));
            }
        }

    }

    public AL_ProfileViewModel(INavigation nav)
    {
        Navigation = nav;
        User = AL_HomePage.CU;
        if (AL_HomePage.CU.Subjects != null)
            Subjects = GlobalFunc.GetSubjectsList(AL_HomePage.CU.Subjects);
    }
    public bool IsTeacher => User.UserType == 2;
    public int BIOHigh => string.IsNullOrEmpty(User.Bio)? 0: GetLineCount(User.Bio) * 40;
    public int SubjHigh => User.Subjects == null ? 0 : (User.Subjects.Count * 40)+20;
    private int GetLineCount(string bio)
    {
        // Split the bio into lines based on newline characters
        var lines = bio.Split(new[] { '\n' }, StringSplitOptions.None);

        int lineCount = 0;

        foreach (var line in lines)
        {
            // Calculate the number of lines based on character constraints
            int charCount = line.Length;
            lineCount += (int)Math.Ceiling(charCount / 20.0);
        }

        return lineCount;
    }

    private ICommand _CopyLongPress;
    public ICommand CopyLongPress
    {
        get { return _CopyLongPress = _CopyLongPress ?? new Command<string>(OnLongPress); }
    }
    private ICommand _EditSubjectCommand;
#pragma warning disable IDE0052 // Remove unread private members
    private int CheckIfNymbersOnly;
#pragma warning restore IDE0052 // Remove unread private members

    public ICommand EditSubjectCommand
    {
        get { return _EditSubjectCommand = _EditSubjectCommand ?? new Command<Subject>(EditSubject); }
    }

    private async void EditSubject(Subject obj)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var li = new ObservableCollection<PopUpSelectItemModel> {
                new PopUpSelectItemModel{Img = "popupselect_20.png" , Desc = "حذف " + obj.Name},
                new PopUpSelectItemModel{Img = "popupselect_21.png" , Desc = "تعديل " + obj.Name},
                new PopUpSelectItemModel{Img = "popupselect_22.png" , Desc = "نسخ إلى الحافظة"}
            };
            var act = await Select.SelectMessage("أختر الأجراء المناسب", li, false, Navigation);
            if (act == li[0].Desc)
            {
                if (AL_HomePage.CU.Subjects.Contains(obj.Name))
                {
                    _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 8, obj.Name);
                    AL_HomePage.CU.Subjects.Remove(obj.Name);
                    User = AL_HomePage.CU;
                }
            }
            else if (act == li[1].Desc)
            {
                if (AL_HomePage.CU.Subjects.Contains(obj.Name))
                {
                    var newname = await GetEntryText.GetEntryTxt("تعديل مادة " + obj.Name, "أسم المادة الجديد", 100, 1, "تراجع", "موافق", Navigation);
                    if (newname != "c")
                    {
                        if (AL_HomePage.CU.Subjects.Contains(newname))
                            await PopupNavigation.Instance.PushAsync(new Msg("هذه المادة موجودة بالفعل.", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (newname.Length < 3)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (newname.Length > 50)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (int.TryParse(newname, out CheckIfNymbersOnly))
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده لا يمكن ان يتكون من أرقام فقط", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else
                        {
                            _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 9, newname, ov: obj.Name);
                            int index = AL_HomePage.CU.Subjects.IndexOf(obj.Name);
                            AL_HomePage.CU.Subjects[index] = newname;
                            User = AL_HomePage.CU;
                        }
                    }
                }
            }
            else if (act == li[2].Desc)
            {
                OnLongPress(obj.Name);
            }
        }, Navigation);
    }

    private void OnLongPress(string i)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Clipboard.Default.SetTextAsync(i);
        });
        GlobalFunc.ToastShow("نسخ إلى الحافظة.");
    }
}
