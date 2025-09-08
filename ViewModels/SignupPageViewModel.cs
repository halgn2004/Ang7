using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class SignupPageViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    public string PageTitle { get; }
    PopUpLoadingViewModel _PopUpLoading;

    public SignupPageViewModel(INavigation _navigation)
    {
        Navigation = _navigation;
        _PopUpLoading = new PopUpLoadingViewModel(_navigation);
        PageTitle = "Legendknight";
        //Code = signupcode;

        Subjects = new ObservableCollection<Subject>
        {
            new Subject { Name = "" }
        };
    }

    private void AddSubject()
    {
        Subjects.Add(new Subject { Name = string.Empty });
        OnPropertyChanged(nameof(subjecthieghts));
    }

    private void RemoveSubject(Subject subject)
    {
        Subjects.Remove(subject);
        OnPropertyChanged(nameof(subjecthieghts));
    }
    void Reset()
    {
        Email = "";
        Name = "";
        Phone = "";
        _ImageSourse = "nouserpic.png";
        Password = "";
        Password2 = "";
        GPS = "";
        streamtosend = null;
        fn = "legendknight";
        //CU = new User();
        Subjects = new ObservableCollection<Subject>
        {
            new Subject { Name = "" }
        };
        pic = "nouserpic.png";
        OnPropertyChanged(nameof(ImageSourse));
        if (IsTeacher)
        {
            BIO = "معلم ذو خبرة في مجال التعليم، شغوف بنقل المعرفة ومساعدة الطلاب على تحقيق إمكانياتهم الكاملة.";
            Subjects = new ObservableCollection<Subject>();
        }
    }
    #region Properties
    public void Set_Email(string email)
    {
        Email = email;
    }
    public void Set_Password(string password)
    {
        Password = password;
    }
    public bool IsTeacher => SignupPage.SUC.UserType == 2;
    public int subjecthieghts => Subjects.Count * 55;


    private ObservableCollection<Subject> _Subjects;
    public ObservableCollection<Subject> Subjects
    {
        get { return _Subjects; }
        set { SetProperty(ref _Subjects, value); }

    }
    private string _email;
    public string Email
    {
        get { return _email; }
        set
        {
            if (SetProperty(ref _email, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }
    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            if (SetProperty(ref _name, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }
    private string _bio = "معلم ذو خبرة في مجال التعليم، شغوف بنقل المعرفة ومساعدة الطلاب على تحقيق إمكانياتهم الكاملة.";
    public string BIO
    {
        get { return _bio; }
        set { SetProperty(ref _bio, value); }
    }

    private string _Phone;
    public string Phone
    {
        get
        {
            return _Phone;
        }
        set
        {
            if (SetProperty(ref _Phone, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }
    private ImageSource _ImageSourse = "nouserpic.png";
    public ImageSource ImageSourse
    {
        get { return _ImageSourse; }
        set
        {
            if (SetProperty(ref _ImageSourse, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }

    private string _password;
    public string Password
    {
        get { return _password; }
        set
        {
            if (SetProperty(ref _password, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
                PWCheck();
            }
        }
    }
    private string _password2;
    public string Password2
    {
        get { return _password2; }
        set
        {
            if (SetProperty(ref _password2, value))
            {
                ((Command)LoginCommand).ChangeCanExecute();
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
        if (string.IsNullOrWhiteSpace(this.Password2) || string.IsNullOrWhiteSpace(this.Password2))
            PWCValidationMsg = false;
        else if (!string.Equals(Password, Password2, StringComparison.Ordinal))
            PWCValidationMsg = true;
        else
            PWCValidationMsg = false;
        //OnPropertyChanged(nameof(PWCValidationMsg));
    }
    #endregion


    #region Commands

    private ICommand _loginCommand;
    public ICommand LoginCommand
    {
        get { return _loginCommand = _loginCommand ?? new Command(LoginAction, CanLoginAction); }
    }


    private ICommand _AttachCommand;
    public ICommand Attach
    {
        get { return _AttachCommand = _AttachCommand ?? new Command(AttachAction); }
    }


    private ICommand _ToLoginCommand;
    public ICommand ToLoginCommand
    {
        get { return _ToLoginCommand = _ToLoginCommand ?? new Command(ToLoginAction); }
    }
    private ICommand _AddSubjectCommand;
    public ICommand AddSubjectCommand
    {
        get { return _AddSubjectCommand = _AddSubjectCommand ?? new Command(AddSubject); }
    }

    private ICommand _RemoveSubjectCommand;
    public ICommand RemoveSubjectCommand
    {
        get { return _RemoveSubjectCommand = _RemoveSubjectCommand ?? new Command<Subject>(RemoveSubject); }
    }

    #endregion


    #region Methods

    async void ToLoginAction()
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var parameters = new ShellNavigationQueryParameters
                {
                    { "from", nameof(SignupPage) },
                };

            if (!string.IsNullOrEmpty(Email))
            {
                parameters.Add("email", Email);
                //Console.WriteLine($"email: {Email} Added to parameters.");
            }
            if (!string.IsNullOrEmpty(Password))
            {
                parameters.Add("password", Password);
                //Console.WriteLine($"password: {Password} Added to parameters.");
            }
            await AppShell.GoToPage(nameof(SigninPage), parameters);
            /*if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                //await Consts.navto(Routes.SigninPage); // Normal navigation to SigninPage
                var parameters = new ShellNavigationQueryParameters
                {
                    { "from", nameof(SignupPage) },
                };
                AppShell.GoToPage(nameof(SigninPage) , parameters);
            }
            else
            {
                // If both are provided, navigate to SigninPage with parameters
                var parameters = new ShellNavigationQueryParameters
                {
                    { "email", Email },
                    { "password", Password },
                    { "from", nameof(SignupPage) }
                };
                AppShell.GoToPage(nameof(SigninPage), parameters);
            }*/
        }, Navigation);
    }
    bool CanLoginAction()
    {
        if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.Name) || string.IsNullOrWhiteSpace(this.Phone) || string.IsNullOrWhiteSpace(this.Password) || string.IsNullOrWhiteSpace(this.Password2) || (Phone.Length != 11 || Phone.Substring(0, 2) != "01") || !string.Equals(Password, Password2, StringComparison.Ordinal) || Password.Length < 6)
        {
            return false;
        }
        return (Validate.Email(Email) && Validate.Name(Name) && Validate.Phone(Phone) && Validate.Password(Password) && Validate.PasswordConfirm(Password2, Password));
        //return LoginButAvalible;
    }
    string GPS = "";
    //SignUpExtraInfoModel ExtraInfo = null;
    async void LoginAction()
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            _PopUpLoading = new PopUpLoadingViewModel(Navigation);
            await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));
            var cp = await CheckEmail(Email);
            if (cp == "3ash.")
            {
                GPS = await GetCurrentLocation();
                getApiResponse(GPS);
            }
            else
            {
                _PopUpLoading.ShowMsg("هذا البريد الإلكتروني بالفعل يملك حساب", "موافق");
            }

        }, Navigation);
    }


    Stream streamtosend = null;
    string fn = "legendknight";
    private async void AttachAction(object obj)
    {
        var li = new ObservableCollection<PopUpSelectItemModel>
    {
        new PopUpSelectItemModel { Img = "popupselect_01.png", Desc = "أختر صوره رمزية" },
        new PopUpSelectItemModel { Img = "popupselect_02.png", Desc = "الكاميرا" },
        new PopUpSelectItemModel { Img = "popupselect_03.png", Desc = "الصور" }
    };

        var act = await Select.SelectMessage("أستبدل الصوره", li, !(ImageSourse.ToString().Contains("nouserpic.png")), Navigation);

        if (act == li[0].Desc)
        {
            string sa = await SelectAvatar.SAvatar(Navigation);
            if (sa != "cancel")
            {
                ImageSourse = pic = sa;
                streamtosend = null;
            }
            return;
        }
        else if (act == li[1].Desc)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take a Photo"
                });

                await LoadPhotoAsync(photo);
            }
            catch (Exception ex) // Handle any exceptions
            {
                // Optionally display an alert to the user or log the error
                await MsgWithIcon.ShowError("لا أستطيع الوصول الي الكاميرا", Navigation, "موافق");
                //await Application.Current.MainPage.DisplayAlert("Error", "Unable to access camera: " + ex.Message, "OK");
            }

        }
        else if (act == li[2].Desc)
        {
            // Pick a photo
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a Photo"
            });

            // Process the photo
            await LoadPhotoAsync(photo);
        }
        else if (act == "cancel")
        {
            // Handle cancellation
            return;
        }
        else if (act == "delete")
        {
            ImageSourse = "nouserpic.png";
            streamtosend = null;
            fn = "legendknight";
        }
    }

    private async Task LoadPhotoAsync(FileResult photo)
    {
        if (photo == null)
        {
            streamtosend = null;
            return; // user canceled file picking
        }

        // Get the stream for the selected photo
        streamtosend = await photo.OpenReadAsync();
        ImageSourse = ImageSource.FromStream(() => streamtosend);
        fn = photo.FullPath; // Full path of the photo
                             // Additional logic can be added here if necessary
    }

    public static async Task<string> CheckEmail(string email)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(email), "\"Email\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckexistsEmail.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    CancellationTokenSource cts;
    async Task<string> GetCurrentLocation()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);

            if (location != null)
            {
                //return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                return $"{location.Latitude},{location.Longitude},{location.Altitude}";
            }
            else
            {
                return "GPS";
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            return "FeatureNotSupportedException";
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            return "FeatureNotEnabledException";
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return "PermissionException";
        }
        catch (Exception ex)
        {
            return "Unable to get location";
            // Unable to get location
        }
    }
    public async Task<string> SignUp(string email, string pw, string phone, string name, string GPS, string pp, bool isteacher = false, string subjects = "", string bio = "")
    {
        try
        {
            //string mylocation = await GetCurrentLocation();
            var content = new MultipartFormDataContent
            {
                { new StringContent(email), "\"Email\"" },
                { new StringContent(pw), "\"pw\"" },
                { new StringContent(name), "\"Name\"" },
                { new StringContent(phone), "\"Phone\"" },
                { new StringContent(GPS), "\"GPS\"" },
                { new StringContent(pp), "\"PP\"" },
                { new StringContent(SignupPage.SUC.ID.ToString()), "\"cid\"" },
                { new StringContent(SignupPage.SUC.Code), "\"code\"" }
            };
            if (isteacher)
            {
                content.Add(new StringContent(subjects), "\"Subj\"");
                content.Add(new StringContent(bio), "\"BIO\"");
            }
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}SignUp.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }

    //User CU = new User();
    string pic = "nouserpic.png";
    public async void getApiResponse(string gps)
    {
        if (streamtosend != null)
        {
            try
            {
                var l = await GlobalFunc.UploadAttachment(streamtosend, fn, 1);
                var jr = JsonConvert.DeserializeObject<UploadFileAPIResponse>(l);
                pic = Consts.APIUrl + "uploaded_files/pp/" + jr.FILENAME;
            }
            catch (Exception ex)
            {
                _PopUpLoading.ShowMsg(ex.Message, "OK");
                return;
            }
        }
        string rv;
        if (IsTeacher)
        {
            rv = await SignUp(this.Email, this.Password, this.Phone, this.Name, gps, pic ,isteacher:true ,subjects: GlobalFunc.GetSubjectsString(Subjects),bio:BIO);
        }else
        {
            rv = await SignUp(this.Email, this.Password, this.Phone, this.Name, gps, pic);
        }
        if (rv == "exists.")
        {
            _PopUpLoading.ShowMsg("هذا البريد الإلكتروني بالفعل يملك حساب", "موافق");
        }
        else if (rv == "wcode.")
        {
            _PopUpLoading.ShowMsg("هذا الكود لم يعد صالح للأستخدام.", "موافق");
            //await Navigation.PushAsync(new HomePage());
            //await Navigation.PopToRootAsync();
            SignupPage.SUC.Used = 1;
            await AppShell.GoToPage(nameof(HomePage));
            //await Consts.navto(nameof(HomePage), clearStack: true); // Navigate with parameters
        }
        else if (rv == "3ash.")
        {
            _PopUpLoading.ShowMsg("تم إنشاء الحساب بنجاح\nبرجاء تسجيل الدخول لتفعيل الحساب", "موافق");
            //await Navigation.PushAsync(new SigninPage(Email, Password));
            // If both are provided, navigate to SigninPage with parameters
            var parameters = new ShellNavigationQueryParameters
                {
                    { "email", Email },
                    { "password", Password },
                    { "from", nameof(SignupPage) }
                };

            SignupPage.SUC.Used = 1;
            await AppShell.GoToPage(nameof(SigninPage), parameters);
            //await Consts.navto(nameof(SigninPage), myparams: parameters); // Navigate with parameters
            Reset();
        }
        else
        {
            _PopUpLoading.ShowMsg("لا يمكن الأتصال بالخادم", "حاول مجددا");
            //_PopUpLoading.ShowMsg($"{Email}\n{Password}\n{Phone}\n{Name}\n{gps}\n{pic}\n{SignupPage.SUC.Code}", "test");
        }
    }

    #endregion
}
