using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class AL_TeacherFilesMangerViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    private User _teacher;

    public List<TeacherFile> AllFiles;
    public TeacherFile CurrentFolder { get; set; }
    public User Teacher
    {
        get => _teacher;
        set => SetProperty(ref _teacher, value);
    }

    public AL_TeacherFilesMangerViewModel(INavigation nav)
    {
        Navigation = nav;
        Teacher = AL_HomePage.CU;
        AllFiles = Teacher.Files;
        AllFiles.Insert(0, new TeacherFile { ID = 0, TypeID = -1, Name = "Home", ParentFolderID = -1 });
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
    private TeacherFile _selectedFile;
    /*public TeacherFile SelectedFile
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
    public bool IsUpAvalible => CurrentFolder.ParentFolderID != -1;//SelectedFile != null ? (SelectedFile.IsFolder) : false;
    //private int UpFolderID = -1;

    /*private void OnLongPress(string i)
    {
        DependencyService.Get<IClipboardService>().CopyToClipboard(i);
        GlobalFunc.ToastShow("نسخ إلى الحافظة.");
    }
    private ICommand _CopyLongPress;
    public ICommand CopyLongPress
    {
        get { return _CopyLongPress = _CopyLongPress ?? new Command<string>(OnLongPress); }
    }*/
    private ICommand _OnSettingCommand;
    public ICommand OnSettingCommand
    {
        get { return _OnSettingCommand = _OnSettingCommand ?? new Command<TeacherFile>(OnFileOptionsPress); }
    }
    private ICommand _UpClicked;
    public ICommand UpClicked
    {
        get { return _UpClicked = _UpClicked ?? new Command(OnUpPress); }
    }
    private ICommand _AddnewClicked;
    public ICommand AddnewClicked
    {
        get { return _AddnewClicked = _AddnewClicked ?? new Command(OnAddnewPress); }
    }
    public ObservableCollection<TeacherFile> GetFilteredFiles(int excludeId)
    {
        // Filter the list to exclude the item with the specified ID
        var filteredFiles = AllFiles
            .Where(file => file.ID != excludeId) // Exclude file with the specified ID
            .ToList();

        // Return as an ObservableCollection
        return new ObservableCollection<TeacherFile>(filteredFiles);
    }
    private async void OnFileOptionsPress(TeacherFile i)
    {
        var li = new ObservableCollection<PopUpSelectItemModel> {
                new PopUpSelectItemModel{Img = "popupselect_40.png" , Desc = "اعادة تسمية"},
                new PopUpSelectItemModel{Img = "popupselect_41.png" , Desc = "نقل الي"},
                new PopUpSelectItemModel{Img = "popupselect_42.png" , Desc = "نسخ الي"},
                new PopUpSelectItemModel{Img = "popupselect_42.png" , Desc = "نسخ"},
                new PopUpSelectItemModel{Img = "popupselect_43.png" , Desc = "حذف"}/*,
                new PopUpSelectItemModel{Img = "popupselect_36.png" , Desc = "أضافة كوبونات"},
                new PopUpSelectItemModel{Img = "popupselect_37.png" , Desc = "التحكم في الكوبونات"}*/
            };
        var act = await Select.SelectMessage("أختر الأجراء المناسب", li, false, Navigation);
        if (act == li[0].Desc)
        {
            var newname = await GetEntryText.GetEntryTxt("أسم جديد", "الأسم الجديد", 100, 1, "تراجع", "موافق", Navigation);
            if (newname != "c")
            {
                if (newname.Length < 3)
                    await PopupNavigation.Instance.PushAsync(new Msg("أسم قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (newname.Length > 50)
                    await PopupNavigation.Instance.PushAsync(new Msg("أسم طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else
                {
                    _ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 1, i.ID, newname);
                    i.Name = newname;
                }
            }
        }
        else if (act == li[1].Desc)
        {
            var selectedFolder = await SelectFolder.Select("نقل الي", GetFilteredFiles(i.ID), Navigation);
            if (selectedFolder != null && int.TryParse(selectedFolder, out int npfid))
            {
                if (npfid == i.ParentFolderID)
                    return;
                _ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 3, i.ID, selectedFolder);
                i.ParentFolderID = npfid;
                MyFiles.Remove(i);
                //OnPropertyChanged(nameof(MyFiles));
                //GlobalFunc.ToastShow(selectedFolder.ToString());
            }
        }
        else if (act == li[2].Desc)
        {
            (int fid, string newname) = await SelectFolderWithEntry.Select("نسخ الي", GetFilteredFiles(i.ID), Navigation, "الأسم الجديد", (i.Name + "- جديد"));
            if (newname.Length < 3)
                await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
            else if (newname.Length > 50)
                await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
            else
            {
                var rv = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 4, i.ID, fid.ToString(), newname);
                if (rv.Substring(0, 5) == "3ash.")
                {
                    int.TryParse(rv.Replace("3ash.", ""), out int nid);
                    var MyNewFile = new TeacherFile { ID = nid, TypeID = i.TypeID, Name = newname, ParentFolderID = fid, Link = i.Link };
                    if (fid == i.ParentFolderID)
                        MyFiles.Add(MyNewFile);
                    AllFiles.Add(MyNewFile);

                    //GlobalFunc.ToastShow($"ID:{nid} , Parent Folder:{pfid}, RV:{t}");
                }
            }
        }
        else if (act == li[3].Desc)
        {
            var newname = await GetEntryText.GetEntryTxt("أسم جديد", "الأسم الجديد", 100, 1, "تراجع", "موافق", Navigation);
            if (newname == "c")
                return;
            if (newname.Length < 3)
                await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
            else if (newname.Length > 50)
                await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
            else
            {
                var rv = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 4, i.ID, i.ParentFolderID.ToString(), newname);
                if (rv.Substring(0, 5) == "3ash.")
                {
                    int.TryParse(rv.Replace("3ash.", ""), out int nid);
                    var MyNewFile = new TeacherFile { ID = nid, TypeID = i.TypeID, Name = newname, ParentFolderID = i.ParentFolderID, Link = i.Link };
                    MyFiles.Add(MyNewFile);
                    AllFiles.Add(MyNewFile);
                }
            }
        }
        else if (act == li[4].Desc)
        {
            if (await ConfirmMsg.ConfirmMessage("هل أنت متأكد أنك تريد حذف هذا الملف ؟", "تأكيد:", "لا", "نعم", Navigation))
            {
                _ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 2, i.ID, "1");
                MyFiles.Remove(i);
                AllFiles.Remove(i);
            }
        }
        /*else if (act == li[5].Desc)
        {
            //createcopouns

            await PopupNavigation.Instance.PushAsync(new CreateCopouns(i.ID));
            //GlobalFunc.TeacherInsertInto_AccessFilesCodes(AL_HomePage.CU.UserID, 0, i.ID, "1000", 6,1,null,50);
        }
        else if (act == li[6].Desc)
        {
            //Managecopouns
        }*/
        //_ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 1,i.ID , name);
        //_ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 3,i.ID , ParentFolderID);
        //_ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 4,i.ID , ParentFolderID , newname);
        //_ = await GlobalFunc.UpdateTeacherFile(AL_HomePage.CU.UserID, 2,i.ID , "ayklam");


        //GlobalFunc.ToastShow(i.Name);
    }

    /*private async void OnTapPress(TeacherFile i)
    {
        GlobalFunc.ToastShow(i.Name);
    }
    private ICommand _OnTapCommand;
    public ICommand OnTapCommand
    {
        get { return _OnTapCommand = _OnTapCommand ?? new Command<TeacherFile>(OnTapPress); }
    }*/
    Stream streamtosend = null;
    string fn = "legendknight";
    PopUpLoadingViewModel _PopUpLoading;

    private async void OnAddnewPress(object obj)
    {
        bool actionExecuted = await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var li = new ObservableCollection<PopUpSelectItemModel> {
                new PopUpSelectItemModel{Img = "popupselect_30.png" , Desc = "مجلد جديد"},
                new PopUpSelectItemModel{Img = "popupselect_31.png" , Desc = "إضافه فيديو"},
                new PopUpSelectItemModel{Img = "popupselect_32.png" , Desc = "إضافه صوره"},
                new PopUpSelectItemModel{Img = "popupselect_33.png" , Desc = "إضافه ملف (PDF)"}
            };
            var li2 = new ObservableCollection<PopUpSelectItemModel> {
                new PopUpSelectItemModel{Img = "popupselect_34.png" , Desc = "رفع"},
                new PopUpSelectItemModel{Img = "popupselect_35.png" , Desc = "رابط مباشر"}
            };
            var act = await Select.SelectMessage("أختر الأجراء المناسب", li, false, Navigation);
            if (act == li[0].Desc)
            {
                var newname = await GetEntryText.GetEntryTxt("مجلد جديد", "أسم المجلد الجديد", 100, 1, "تراجع", "موافق", Navigation);
                if (newname != "c")
                {
                    if (newname.Length < 3)
                        await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                    else if (newname.Length > 50)
                        await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                    else
                    {
                        int pfid = CurrentFolder.ParentFolderID == -1 ? 0 : CurrentFolder.ID;
                        var rv = await GlobalFunc.TeacherNewFolder(AL_HomePage.CU.UserID, newname, pfid);
                        if (rv.Substring(0, 5) == "3ash.")
                        {
                            int.TryParse(rv.Replace("3ash.", ""), out int nid);
                            var MyNewFolder = new TeacherFile { ID = nid, TypeID = -1, Name = newname, ParentFolderID = pfid };
                            MyFiles.Add(MyNewFolder);
                            AllFiles.Add(MyNewFolder);

                            //GlobalFunc.ToastShow($"ID:{nid} , Parent Folder:{pfid}, RV:{t}");
                        }
                        else if (rv == "error01")
                        {
                            //
                        }
                        else if (rv == "error300")
                        {
                            //
                        }
                        else if (rv == "error301")
                        {
                            //
                        }
                        else
                        {
                            //
                        }
                        /*
                        int index = AL_HomePage.CU.Subjects.IndexOf(obj.Name);
                        AL_HomePage.CU.Subjects[index] = newname;
                        User = AL_HomePage.CU;*/
                    }
                    /*if (AL_HomePage.CU.Subjects.Contains(newname))
                        await PopupNavigation.Instance.PushAsync(new Msg("هذه المادة موجودة بالفعل.", "خطأ", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
                    else if (newname.Length < 3)
                        await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
                    else if (newname.Length > 50)
                        await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
                    else if (int.TryParse(newname, out CheckIfNymbersOnly))
                        await PopupNavigation.Instance.PushAsync(new Msg("أسم الماده لا يمكن ان يتكون من أرقام فقط", "خطأ", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
                    else
                    {
                        _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 9, newname, ov: obj.Name);
                        int index = AL_HomePage.CU.Subjects.IndexOf(obj.Name);
                        AL_HomePage.CU.Subjects[index] = newname;
                        User = AL_HomePage.CU;
                    }*/
                }
            }
            else if (act == li[1].Desc)
            {
                var act2 = await Select.SelectMessage("أختر الأجراء المناسب", li2, false, Navigation);
                if (act2 == li2[0].Desc)
                {
                    // Upload video
                    var pli = new ObservableCollection<PopUpSelectItemModel> {
                            new PopUpSelectItemModel{ Img = "popupselect_02.png", Desc = "الكاميرا" },
                            new PopUpSelectItemModel{ Img = "popupselect_03.png", Desc = "الفيديوهات" }
                        };

                    var pact = await Select.SelectMessage("أختر الفيديو", pli, false, Navigation);
                    FileResult file = null;

                    if (pact == pli[0].Desc)
                    {
                        // Check if camera video recording is supported
                        if (!MediaPicker.Default.IsCaptureSupported)
                        {
                            //await DisplayAlert("Error", "Camera capture is not supported on this device.", "OK");
                            GlobalFunc.ToastShow("Camera capture is not supported on this device.");
                            return; // Handle if camera is not supported
                        }
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam*2);
#endif

                        fn = $"{Guid.NewGuid()}.mp4";
                        try
                        {
                            // Capture video using the camera
                            file = await MediaPicker.CaptureVideoAsync(new MediaPickerOptions { Title = fn });
                        }
                        catch (Exception ex)
                        {
                            // Handle any exception from camera capture
                            GlobalFunc.ToastShow($"Failed to capture video: {ex.Message}");
                            return;
                        }
                    }
                    else if (pact == pli[1].Desc)
                    {
                        try
                        {
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam*2);
#endif
                            // Pick a video from the gallery
                            file = await MediaPicker.PickVideoAsync(new MediaPickerOptions { Title = "Pick a video" });
                        }
                        catch (Exception ex)
                        {
                            // Handle any exception from file picking
                            GlobalFunc.ToastShow($"Failed to pick video: {ex.Message}");
                            return;
                        }
                    }
                    else if (pact == "cancel")
                    {
                        // Handle cancellation
                        return;
                    }

                    if (file == null)
                    {
                        streamtosend = null;
                        return; // user canceled file picking
                    }
                    else
                    {
                        // Get the stream of the selected video
                        //using var stream = await file.OpenReadAsync();
                        streamtosend = await file.OpenReadAsync();
                        fn = file.FullPath; // Get the file path
                        getApiResponse(1);  // Call your API response function
                    }
                }
                else if (act2 == li2[1].Desc)
                {
                    //direct link
                    (string newname, string newlink) = await Get2EntryText.Get2EntryTxt("فيديو جديد", "أسم الفيديو الجديد", "الرابط المباشر الخاص بي الفيديو الجديد", 100, 1, "تراجع", "موافق", Navigation);
                    if (newname != "c")
                    {
                        if (newname.Length < 3)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الفيديو قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (newname.Length > 50)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الفيديو طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (GlobalFunc.GetFileTypefromurl(newlink) != 1)
                            await PopupNavigation.Instance.PushAsync(new Msg("رابط غير صحيح", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else
                        {
                            int pfid = CurrentFolder.ParentFolderID == -1 ? 0 : CurrentFolder.ID;
                            var rv = await GlobalFunc.TeacherNewFileFromUrl(AL_HomePage.CU.UserID, newname, pfid, 1, newlink);
                            if (rv.Substring(0, 5) == "3ash.")
                            {
                                int.TryParse(rv.Replace("3ash.", ""), out int nid);
                                var MyNewVid = new TeacherFile { ID = nid, TypeID = 1, Name = newname, ParentFolderID = pfid, Link = newlink };
                                MyFiles.Add(MyNewVid);
                                AllFiles.Add(MyNewVid);

                                //GlobalFunc.ToastShow($"ID:{nid} , Parent Folder:{pfid}, RV:{t}");
                            }
                            else if (rv == "error01")
                            {
                                //
                            }
                            else if (rv == "error300")
                            {
                                //
                            }
                            else if (rv == "error301")
                            {
                                //
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    //direct link end
                }
            }
            else if (act == li[2].Desc)
            {
                var act2 = await Select.SelectMessage("أختر الأجراء المناسب", li2, false, Navigation);
                if (act2 == li2[0].Desc)
                {
                    // upload pic
                    var pli = new ObservableCollection<PopUpSelectItemModel>
                    {
                        new PopUpSelectItemModel { Img = "popupselect_02.png", Desc = "الكاميرا" },
                        new PopUpSelectItemModel { Img = "popupselect_03.png", Desc = "الصور" }
                    };

                    var pact = await Select.SelectMessage("أختر الصوره", pli, false, Navigation);
                    FileResult file = null;

                    if (pact == pli[0].Desc)
                    {
                        // Check if camera is available
                        if (!MediaPicker.Default.IsCaptureSupported)
                        {
                            // Display alert about camera not being available
                            return;
                        }
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam*2);
#endif

                        file = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                        {
                            Title = $"{Guid.NewGuid()}.jpg"
                        });
                    }
                    else if (pact == pli[1].Desc)
                    {
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam*2);
#endif
                        // Pick a photo from gallery
                        file = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                        {
                            Title = "Pick a photo"
                        });
                    }
                    else if (act2 == "cancel")
                    {
                        // Handle cancellation
                        return;
                    }

                    if (file == null)
                    {
                        // User canceled photo picking
                        streamtosend = null;
                        return;
                    }
                    else
                    {
                        // Get stream of the selected image
                        //using var stream = await file.OpenReadAsync();
                        streamtosend = await file.OpenReadAsync();
                        fn = file.FullPath; // Path to the file
                        getApiResponse(2);  // Call your API response function
                    }
                }

                else if (act2 == li2[1].Desc)
                {
                    //direct link
                    (string newname, string newlink) = await Get2EntryText.Get2EntryTxt("صوره جديده", "أسم الصوره الجديده", "الرابط المباشر الخاص بي الصوره الجديده", 100, 1, "تراجع", "موافق", Navigation);
                    if (newname != "c")
                    {
                        if (newname.Length < 3)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الصوره قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (newname.Length > 50)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الصوره طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (GlobalFunc.GetFileTypefromurl(newlink) != 2)
                            await PopupNavigation.Instance.PushAsync(new Msg("رابط غير صحيح", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else
                        {
                            int pfid = CurrentFolder.ParentFolderID == -1 ? 0 : CurrentFolder.ID;
                            var rv = await GlobalFunc.TeacherNewFileFromUrl(AL_HomePage.CU.UserID, newname, pfid, 2, newlink);
                            if (rv.Substring(0, 5) == "3ash.")
                            {
                                int.TryParse(rv.Replace("3ash.", ""), out int nid);
                                var MyNewPic = new TeacherFile { ID = nid, TypeID = 2, Name = newname, ParentFolderID = pfid, Link = newlink };
                                MyFiles.Add(MyNewPic);
                                AllFiles.Add(MyNewPic);
                                //GlobalFunc.ToastShow($"ID:{nid} , Parent Folder:{pfid}, RV:{t}");
                            }
                            else if (rv == "error01")
                            {
                                //
                            }
                            else if (rv == "error300")
                            {
                                //
                            }
                            else if (rv == "error301")
                            {
                                //
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    //direct link end
                }
            }
            else if (act == li[3].Desc)
            {
                var act2 = await Select.SelectMessage("أختر الأجراء المناسب", li2, false, Navigation);
                if (act2 == li2[0].Desc)
                {
                    //upload pdf
                    FileResult fileResult = null;
                    try
                    {
                        var pickOptions = new PickOptions
                        {
                            PickerTitle = "اختر ملف PDF", // Title for the file picker
                            FileTypes = FilePickerFileType.Pdf // Restricting to PDF files
                        };
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam*2);
#endif

                        fileResult = await FilePicker.PickAsync(pickOptions);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, such as user canceling the operation
                        Console.WriteLine(ex);
                    }
                    if (fileResult == null)
                    {
                        streamtosend = null;
                        return; // user canceled file picking
                    }
                    else
                    {
                        var stream = await fileResult.OpenReadAsync();
                        streamtosend = stream;
                        fn = fileResult.FullPath;
                        getApiResponse(3);
                    }
                }
                else if (act2 == li2[1].Desc)
                {
                    //direct link
                    (string newname, string newlink) = await Get2EntryText.Get2EntryTxt("ملف جديد", "أسم الملف الجديد", "الرابط المباشر الخاص بي الملف الجديد", 100, 1, "تراجع", "موافق", Navigation);
                    if (newname != "c")
                    {
                        if (newname.Length < 3)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الملف قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (newname.Length > 50)
                            await PopupNavigation.Instance.PushAsync(new Msg("أسم الملف طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else if (GlobalFunc.GetFileTypefromurl(newlink) != 3)
                            await PopupNavigation.Instance.PushAsync(new Msg("رابط غير صحيح", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        else
                        {
                            int pfid = CurrentFolder.ParentFolderID == -1 ? 0 : CurrentFolder.ID;
                            var rv = await GlobalFunc.TeacherNewFileFromUrl(AL_HomePage.CU.UserID, newname, pfid, 3, newlink);
                            if (rv.Substring(0, 5) == "3ash.")
                            {
                                int.TryParse(rv.Replace("3ash.", ""), out int nid);
                                var MyNewPic = new TeacherFile { ID = nid, TypeID = 3, Name = newname, ParentFolderID = pfid, Link = newlink };
                                MyFiles.Add(MyNewPic);
                                AllFiles.Add(MyNewPic);

                                //GlobalFunc.ToastShow($"ID:{nid} , Parent Folder:{pfid}, RV:{t}");
                            }
                            else if (rv == "error01")
                            {
                                //
                            }
                            else if (rv == "error300")
                            {
                                //
                            }
                            else if (rv == "error301")
                            {
                                //
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    //direct link end
                }
            }

        }, Navigation);
        if (!actionExecuted)
        {
            return;
        }
    }
    string pic = "LegendKnight";
    public async void getApiResponse(int type)
    {
        if (streamtosend == null)
            return;

        _PopUpLoading = new PopUpLoadingViewModel(Navigation);
        await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));
        try
        {
            var l = await GlobalFunc.UploadAttachment(streamtosend, fn, 2);
            var jr = JsonConvert.DeserializeObject<UploadFileAPIResponse>(l);
            pic = Consts.APIUrl + "uploaded_files/data/" + jr.FILENAME;
        }
        catch (Exception ex)
        {
            _PopUpLoading.ShowMsg(ex.Message, "OK");
            return;
        }
        int pfid = CurrentFolder.ParentFolderID == -1 ? 0 : CurrentFolder.ID;
        var rv = await GlobalFunc.TeacherNewFileFromUrl(AL_HomePage.CU.UserID, Path.GetFileNameWithoutExtension(fn), pfid, type, pic);

        if (rv.Substring(0, 5) == "3ash.")
        {
            int.TryParse(rv.Replace("3ash.", ""), out int nid);
            var MyNewPic = new TeacherFile { ID = nid, TypeID = type, Name = Path.GetFileNameWithoutExtension(fn), ParentFolderID = pfid, Link = pic };
            MyFiles.Add(MyNewPic);
            AllFiles.Add(MyNewPic);
            //reset was here
        }
        else if (rv == "error01")
        {
            //_PopUpLoading.ShowMsg("هذا البريد الإلكتروني بالفعل يملك حساب", "موافق");

        }
        else if (rv == "error300")
        {
            //
        }
        else if (rv == "error301")
        {
            //
        }
        else
        {
            //
        }
            //reset
            streamtosend = null;
            fn = "legendknight";
            pic = "LegendKnight";
            _PopUpLoading.Done();
    }
    private void OnUpPress()
    {
        if (CurrentFolder.ParentFolderID == -1)
            return;
        MyFiles = new ObservableCollection<TeacherFile>(GetFiles(CurrentFolder.ParentFolderID));
        CurrentFolder = AllFiles.Where(f => f.ID == CurrentFolder.ParentFolderID).FirstOrDefault();
        OnPropertyChanged(nameof(IsUpAvalible));
    }


    private IEnumerable<TeacherFile> GetFiles(int parentFolderId)
    {
        return AllFiles.Where(f => f.ParentFolderID == parentFolderId);
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
            switch (selectedFile.TypeID)
            {
                case 1:
                    await Navigation.PushAsync(new PlayVideoPage(selectedFile.Name, selectedFile.Link));
                    break;
                case 2:
                    await Navigation.PushAsync(new ImageViewPage(selectedFile.Name, selectedFile.Link));
                    break;
                case 3: 
                    await Navigation.PushAsync(new PDFViewPage(selectedFile.Name, selectedFile.Link));
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
