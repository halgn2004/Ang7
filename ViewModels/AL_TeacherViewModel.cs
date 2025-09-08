using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using RGPopup.Maui.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class AL_TeacherViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    private Teacher _teacher;

    public List<TeacherFile> AllFiles;
    public TeacherFile CurrentFolder { get; set; }
    public Teacher Teacher
    {
        get => _teacher;
        set => SetProperty(ref _teacher, value);
    }

    public AL_TeacherViewModel(Teacher t ,INavigation nav)
    {
        Navigation = nav;
        Teacher = t;
        AllFiles = Teacher.Files;
        AllFiles.Insert(0 , new TeacherFile { ID = 0, TypeID = -1, Name = "Home", ParentFolderID = -1 });
        MyFiles = new ObservableCollection<TeacherFile>(GetFiles(0));
        CurrentFolder = AllFiles[0];
        //Subjects = GlobalFunc.GetSubjectsList(t.Subjects);
    }
    private ObservableCollection<TeacherFile> _myFiles = new ObservableCollection<TeacherFile>();
    public ObservableCollection<TeacherFile> MyFiles
    {
        get => _myFiles;
        set => SetProperty(ref _myFiles, value);
    }
    /*private TeacherFile _selectedFile;
    public TeacherFile SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (_selectedFile != value)
            {
                _selectedFile = value;
                OnPropertyChanged();
                OnFileSelected(_selectedFile);
            }
        }
    }*/
    public int BIOHigh => string.IsNullOrEmpty(Teacher.Bio)? 0: GetLineCount(Teacher.Bio) * 30;//40
    public int SubjHigh => Teacher.Subjects == null ? 0 : (Teacher.Subjects.Count * 45)+20;

    public bool IsUpAvalible => CurrentFolder.ParentFolderID != -1;//SelectedFile != null ? (SelectedFile.IsFolder) : false;
    //private int UpFolderID = -1;
    public ObservableCollection<Subject> Subjects => GlobalFunc.GetSubjectsList(Teacher.Subjects);
    private int GetLineCount(string bio)
    {
        var lines = bio.Split(new[] { '\n' }, StringSplitOptions.None);
        int lineCount = 0;
        foreach (var line in lines)
        {
            lineCount += (int)Math.Ceiling(line.Length / 20.0);
        }
        return lineCount;
    }
    private void OnLongPress(string i)
    {

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Clipboard.Default.SetTextAsync(i);
        });
        GlobalFunc.ToastShow("نسخ إلى الحافظة.");
    }
    private ICommand _CopyLongPress;
    public ICommand CopyLongPress
    {
        get { return _CopyLongPress = _CopyLongPress ?? new Command<string>(OnLongPress); }
    }
    private ICommand _UpClicked;
    public ICommand UpClicked
    {
        get { return _UpClicked = _UpClicked ?? new Command(OnUpPress); }
    }
    private ICommand _ItemTappedCommand;
    public ICommand ItemTappedCommand
    {
        get { return _ItemTappedCommand = _ItemTappedCommand ?? new Command<TeacherFile>(OnItemTapped); }
    }
    private async void OnItemTapped(TeacherFile selectedFile)
    {
        selectedFile.BorderColor = Colors.LimeGreen;
        // Execute your main function
        OnItemTappedFun(selectedFile);
        await Task.Delay(200);
        selectedFile.BorderColor = Color.FromRgb(246, 248, 249);
    }
    private async void OnItemTappedFun(TeacherFile selectedFile)
    {
        if (selectedFile == null)
            return;
        /*if (!await Validate.UserSession())
        {
            await AppShell.GoToPage(nameof(HomePage));
            return;
        }*/
        //SelectedFile = null;
        if (selectedFile.IsSecured)
        {
            string rv = await GlobalFunc.CheckAccessFiles(AL_HomePage.CU.UserID.ToString(), selectedFile.ID.ToString());

            if (rv != "succ")
            {
                var code = await GetEntryText.GetEntryTxt("من فضلك ادخل كود الملف", "كود الملف", 500, 1, "الغاء", "موافق", Navigation);
                if (code != "c")
                {
                    if (string.IsNullOrEmpty(code) || code.Length < 3)
                        return;
                    string rv2 = await GlobalFunc.CheckAccessFiles(AL_HomePage.CU.UserID.ToString(), selectedFile.ID.ToString(), c: code);
                    if (rv2 == "succ")
                    {
                        OpenFile(selectedFile);
                        return;
                    }
                    else if (rv2 == "error05")
                    {
                        GlobalFunc.ToastShow("لقد تم أستخدام الحد الأقصي لهذا الكود");
                        return;
                    }
                    else if (rv2 == "error01")
                    {

                        GlobalFunc.ToastShow("لقد تم أنتهاء فترة صالحية هذا الكود");
                        return;
                    }
                    else if (rv2 == "error03")
                    {

                        GlobalFunc.ToastShow("كود خطأ");
                        return;
                    }
                }
            }
            else // mtactive code asln
            {
                OpenFile(selectedFile);
            }
        }
        else
        {
            OpenFile(selectedFile);
        }
    }
    private void OnUpPress()
    {
        if (CurrentFolder.ParentFolderID == -1)
            return;
        //MyFiles?.Clear();
        MyFiles = new ObservableCollection<TeacherFile>(GetFiles(CurrentFolder.ParentFolderID));
        CurrentFolder = AllFiles.Where(f => f.ID == CurrentFolder.ParentFolderID).FirstOrDefault();
        OnPropertyChanged(nameof(IsUpAvalible));
    }

    /*private ICommand _FileSelectedCommand;
public ICommand FileSelectedCommand
{
get { return _FileSelectedCommand = _FileSelectedCommand ?? new Command<TeacherFile>(OnFileSelected); }
}*/

    private IEnumerable<TeacherFile> GetFiles(int parentFolderId)
    {
        return AllFiles.Where(f => f.ParentFolderID == parentFolderId);
    }

    private async void OpenFile(TeacherFile selectedFile)
    {
        if (selectedFile.TypeID == -1) // Folder type
        {
            OpenFolder(selectedFile);
        }
        else
        {
            await TapEventHandler.HandleTapEvent(true, async () =>
            {
                await OpenNonFolderFile(selectedFile);
            }, Navigation);
        }
    }

    private void OpenFolder(TeacherFile selectedFolder)
    {
        try
        {
            CurrentFolder = selectedFolder;

            /*MainThread.BeginInvokeOnMainThread(() =>
            {
                MyFiles.Clear();
                MyFiles.AddRange(GetFiles(selectedFolder.ID));
            });*/
            MyFiles = new ObservableCollection<TeacherFile>(GetFiles(selectedFolder.ID));
            OnPropertyChanged(nameof(IsUpAvalible));
        }
        catch (ObjectDisposedException ex)
        {
            Debug.WriteLine($"ObjectDisposedException: {ex.ObjectName} has already been disposed.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error in OpenFolder: {ex.Message}");
        }
    }


    private async Task OpenNonFolderFile(TeacherFile selectedFile)
    {



        try
        {
            /*var parameters = new ShellNavigationQueryParameters
                    {
                        { "from", nameof(AL_TeacherView) },
                        { "title", selectedFile.Name },
                        { "Source", selectedFile.Link }
                    };*/
            var popVm = new PopUpLoadingViewModel(Navigation);
            switch (selectedFile.TypeID)
            {
                case 1: // Video
                        // var videoplayer = new VideoPlayer(selectedFile.Name, selectedFile.Link);
                        //await PopupNavigation.Instance.PushAsync(videoplayer);
                        //await AppShell.GoToPage(nameof(PlayVideoPage), parameters);

                    await PopupNavigation.Instance.PushAsync(new Loading(popVm));
                    int videocount = await GlobalFunc.VideoCount(AL_HomePage.CU.UserID, selectedFile.ID);
                    if (videocount >= 4)
                    {
                        if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Any())
                            await PopupNavigation.Instance.PopAsync();

                        GlobalFunc.ToastShow("لقد استنفذت عدد مشاهداتك للفيديو");
                        break;
                    }

                    await Navigation.PushAsync(new PlayVideoPage(selectedFile.Name, selectedFile.Link));
                    break;
                case 2: // Image
                    //var imageViewer = new ImageViewer(selectedFile.Name, selectedFile.Link);
                    //await PopupNavigation.Instance.PushAsync(imageViewer);
                    //AppShell.GoToPage(nameof(ImageViewPage), parameters);
                    await Navigation.PushAsync(new ImageViewPage(selectedFile.Name, selectedFile.Link));
                    break;
                case 3: // PDF
                        //var pdfViewer = new PDFViewer(selectedFile.Name, selectedFile.Link);
                        //await PopupNavigation.Instance.PushAsync(pdfViewer);
                        //await AppShell.GoToPage(nameof(PDFViewPage), parameters);
                    await PopupNavigation.Instance.PushAsync(new Loading(popVm));
                    await Navigation.PushAsync(new PDFViewPage(selectedFile.Name, selectedFile.Link));
                    break;
                case 4:
                    GlobalFunc.ToastShow("لقد استنفذت عدد مشاهداتك للفيديو"); ;
                    break;
                default:
                    GlobalFunc.ToastShow("ملف غير معرف");
                    break;
            }
        }
        catch (ObjectDisposedException ex)
        {
            Debug.WriteLine("Attempted to access a disposed object: " + ex.ObjectName);
        }
    }

}
