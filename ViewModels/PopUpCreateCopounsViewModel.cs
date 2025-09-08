using System.Windows.Input;
using Ang7.Views;
using Ang7.Helpers;

namespace Ang7.ViewModels;

public partial class PopUpCreateCopounsViewModel : BaseViewModel
{
    private readonly INavigation Navigation;

    private int copounscount;
    public int CopounsCount
    {
        get { return copounscount; }
        set { SetProperty(ref copounscount, value); }
    }

    private int noofusers;
    public int NoOfUsers
    {
        get { return noofusers; }
        set { SetProperty(ref noofusers, value); }
    }

    private int codelenght;
    public int CodeLength
    {
        get { return codelenght; }
        set { SetProperty(ref codelenght, value); }
    }

    private string pwpattern;
    public string PwPattern
    {
        get { return pwpattern; }
        set { SetProperty(ref pwpattern, value); }
    }
    private DateTime selecteddate;
    public DateTime SelectedDate
    {
        get { return selecteddate; }
        set { SetProperty(ref selecteddate, value); }
    }

    public DateTime MinDate => DateTime.Today;
    public DateTime MaxDate => DateTime.Today.AddYears(3);

    private int FFID;
    public PopUpCreateCopounsViewModel(int ForFileID)
    {
        //Navigation = _navigation;
        //Mindate = DateTime.Today;
        //MaxDate = DateTime.Today.AddYears(3);
        FFID = ForFileID;
        PwPattern = Settings.PWPatters_Pattern ?? "1000"; // Default to "1000" if null
        CodeLength = Settings.PWPatters_CodeLenght > 0 ? Settings.PWPatters_CodeLenght : 8; // Default to 8
        SelectedDate = Settings.PWPatters_ValidTo > DateTime.MinValue ? Settings.PWPatters_ValidTo : DateTime.Today;
        CopounsCount = Settings.PWPatters_CopounsCount > 0 ? Settings.PWPatters_CopounsCount : 1;
    }
    /*private ICommand _ExecuteCommand;
    public ICommand ExecuteCommand
    {
        get { return _ExecuteCommand = _ExecuteCommand ?? new Command(ExecuteAction); }
    }*/
    private ICommand _ExecuteCommand;
    public ICommand ExecuteCommand => _ExecuteCommand ??= new Command(ExecuteAction);

    private async void ExecuteAction()
    {
        if (!ValidateInput()) return;

        await GlobalFunc.TeacherInsertInto_AccessFilesCodes(
                AL_HomePage.CU.UserID,
                0,
                FFID,
                PwPattern,
                CodeLength,
                NoOfUsers,
                SelectedDate.ToString("yyyy-MM-dd"),
                CopounsCount
            );

        Settings.PWPatters_ValidTo = SelectedDate;
        Settings.PWPatters_CopounsCount = CopounsCount;
        Settings.PWPatters_Pattern = PwPattern;
        Settings.PWPatters_CodeLenght = CodeLength;
    }
    private bool ValidateInput()
    {
        if (CopounsCount <= 0 || CodeLength <= 0 || NoOfUsers <= 0)
        {
            Application.Current.MainPage.DisplayAlert("Error", "Values must be positive integers.", "OK");
            return false;
        }
        if (string.IsNullOrWhiteSpace(PwPattern))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Password pattern cannot be empty.", "OK");
            return false;
        }
        if (SelectedDate < MinDate || SelectedDate > MaxDate)
        {
            Application.Current.MainPage.DisplayAlert("Error", "Selected date is out of range.", "OK");
            return false;
        }

        return true;
    }
}