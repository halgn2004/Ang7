using System.Collections.ObjectModel;
using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;
namespace Ang7.ViewModels;

public class AL_HomePageViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    public AL_HomePageViewModel(INavigation navigation)
    {

        Navigation = navigation;
        NavigateToDetailPageCommand = new Command<Teacher>(async (param) => await ExeccuteNavigateToDetailPageCommand(param));
        RefreshCommand = new Command(async () => await TapEventHandler.HandleTapEvent(true, async () =>
        {
            await ExeccuteRefreshCommand();
        }, Navigation)
        );
        OpenSearchBarCommand = new Command(() => ExeccuteSearchCommand());
        PerformSearchCommand = new Command((t) => ExeccutePerformSearchCommand(t));
        OnPropertyChanged(nameof(IsAdmin));
        _ = ExeccuteRefreshCommand();
        MyMenu = GetMenus();


        /*OnCartButtonClickedCommand = new Command((t) => {
            //CleanAllCartProducts(0);
            //Navigation.PushAsync(new BillDetailPage(AllCartProducts, this));
        });*/
        OnMessageButtonClickedCommand = new Command(async (t) => {
            await TapEventHandler.HandleTapEvent(false, async () =>
            {
                await Navigation.PushAsync(new FeedbackMsg(MyFBs));
            }, Navigation);
        });
        /*try {
        }
        catch (Exception ex) { PopupNavigation.Instance.PushAsync(new Msg(ex.Message + "\n\n" + ex.StackTrace, "01", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق")); }
        */
    }

    private void ExeccutePerformSearchCommand(object t)
    {
        string te = (string)t;
        if (AllTeachers == null || !AllTeachers.Any())
            return;
        var lr = AllTeachers.Where(p => p.Name.Contains(te, StringComparison.OrdinalIgnoreCase));//, StringComparison.OrdinalIgnoreCase
        if(lr.Count() != SearchTeachers.Count)
            SearchTeachers = new ObservableCollection<Teacher>(lr);
        //App.Current.MainPage.DisplayAlert("test",te,"ok");
    }

    private void ExeccuteSearchCommand()
    {
        IsSearchMode = true;
        SearchTeachers = new ObservableCollection<Teacher>(AllTeachers);
    }
    public void CloseSearchMode()
    {
        IsSearchMode = false;
        SearchTeachers = new ObservableCollection<Teacher>();
    }


    bool firststart = true;


    private bool _IsSearchMode = false;
    public bool IsAnySearchRes => SearchTeachers != null && SearchTeachers.Count() > 0;
    public bool IsSearchIcon => !IsSearchMode && !IsEmpty;
    public bool IsSearchMode
    {
        get { return _IsSearchMode; }
        set { SetProperty(ref _IsSearchMode, value); }
    }
    private int _RefreshImgRotation;
    public int RefreshImgRotation
    {
        get { return _RefreshImgRotation; }
        set { SetProperty(ref _RefreshImgRotation, value); }
    }
    private bool _IsEmpty;
    public bool IsEmpty
    {
        get { return _IsEmpty; }
        set { SetProperty(ref _IsEmpty, value); }
    }

    public bool Isfbmsg => MyFBs != null && MyFBs.Count > 0;//{ get; set; }
    public bool IsAdmin => AL_HomePage.CU.UserType == 1;
    public bool IsGuest => AL_HomePage.CU.UserType == -1;
    public ObservableCollection<Models.Menu> MyMenu { get; set; }
    private ObservableCollection<Models.Menu> GetMenus()
    {
        switch (AL_HomePage.CU.UserType)
        {
            case 1:
                return new ObservableCollection<Models.Menu>
                {
                    new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = "mi_profile.png" },
                    new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                    new Models.Menu { ID = 3, Name = "تسجيل خروج", Icon = "mi_signout.png" },
                    new Models.Menu { ID = 8, Name = "المقترحات والشكاوي", Icon = "mi_feedback.png" },
                };
                //break;
            case 2:
                return new ObservableCollection<Models.Menu>
                {
                    new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = "mi_profile.png" },
                    new Models.Menu { ID = 5, Name = "مدير الملفات", Icon = "mi_filemanager.png" },
                    new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                    new Models.Menu { ID = 3, Name = "المقترحات والشكاوي", Icon = "mi_feedback.png" },
                    new Models.Menu { ID = 4, Name = "تسجيل خروج", Icon = "mi_signout.png" }
                };
                //break;
            case 3:
                return new ObservableCollection<Models.Menu>
                {
                    new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = "mi_profile.png" },
                    new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                    new Models.Menu { ID = 3, Name = "المقترحات والشكاوي", Icon = "mi_feedback.png" },
                    new Models.Menu { ID = 4, Name = "تسجيل خروج", Icon = "mi_signout.png" }
                };
                //break;
            default:
                return new ObservableCollection<Models.Menu>
                {
                    new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                    new Models.Menu { ID = 4, Name = "تسجيل خروج", Icon = "mi_signout.png" }
                };
                //break;
        }
        /*if (IsAdmin)
        {
            return new ObservableCollection<Models.Menu>
            {
                //new Models.Menu { Name = "أستكشف", Icon = "mi_explore.png" },
                new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = "mi_profile.png" },
                //new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = AL_HomePage.CU.PP },
                new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                new Models.Menu { ID = 3, Name = "تسجيل خروج", Icon = "mi_signout.png" },

                //new Models.Menu { ID = 4, Name = "أضافة منتج", Icon = "ap_add.png" },
                //new Models.Menu { ID = 5, Name = "الطلبيات", Icon = "ap_orders.png" },
                //new Models.Menu { ID = 6, Name = "طلبات أنشاء حساب", Icon = "mi_caccreq.png"},
                //new Models.Menu { ID = 7, Name = "طلبات مرفوضة", Icon = "mi_caccreq2.png" },
                new Models.Menu { ID = 8, Name = "المقترحات والشكاوي", Icon = "mi_feedback.png" },
                //new Models.Menu { ID = 9, Name = "طلبات تغيير باسورد", Icon = "mi_changepw.png" },
                //new Models.Menu { ID = 10, Name = "تاريخ الطلبيات", Icon = "mi_rhis.png" },
            };
        }
        else if (IsGuest)
        {
            return new ObservableCollection<Models.Menu>
            {
                new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                new Models.Menu { ID = 4, Name = "تسجيل خروج", Icon = "mi_signout.png" }
            };
        }
        else
        {
            return new ObservableCollection<Models.Menu>
            {
                //new Models.Menu { Name = "أستكشف", Icon = "mi_explore.png" },
                new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = "mi_profile.png" },
                new Models.Menu { ID = 5, Name = "مدير الملفات", Icon = "mi_filemanager.png" },
                //new Models.Menu { ID = 1, Name = "الملف الشخصي", Icon = AL_HomePage.CU.PP },
                new Models.Menu { ID = 2, Name = "الإعدادات", Icon = "mi_settings.png" },
                new Models.Menu { ID = 3, Name = "المقترحات والشكاوي", Icon = "mi_feedback.png" },
                new Models.Menu { ID = 4, Name = "تسجيل خروج", Icon = "mi_signout.png" }
            };
        }*/
    }

    private ObservableCollection<Teacher> _AllTeachers;
    public ObservableCollection<Teacher> AllTeachers
    {
        get
        {
            return _AllTeachers;
        }
        set
        {
            if (SetProperty(ref _AllTeachers, value))
            {
                //OnPropertyChanged(nameof(Count));
            }
        }
    }
    private ObservableCollection<Teacher> _SearchTeachers;
    public ObservableCollection<Teacher> SearchTeachers
    {
        get
        {
            return _SearchTeachers;
        }
        set
        {
            if (SetProperty(ref _SearchTeachers, value))
            {
                OnPropertyChanged(nameof(IsAnySearchRes));
            }
        }
    }
    private ObservableCollection<Teacher> _Teachers;
    public ObservableCollection<Teacher> Teachers
    {
        get
        {
            return _Teachers;
        }
        set
        {
            if (SetProperty(ref _Teachers, value))
            {
                //OnPropertyChanged(nameof(Count));
            }
        }
    }

    private ObservableCollection<Models.Feedback> _AllFBs = new ObservableCollection<Models.Feedback>();
    public ObservableCollection<Models.Feedback> AllFBs
    {
        get
        {
            return _AllFBs;
        }
        set
        {
            SetProperty(ref _AllFBs, value);
            if (value != null)
                MyMenu.Where(c => c.ID == 8).FirstOrDefault().Badge = (value.Count() > 0) ? value.ToString() : null;
        }
    }
    /*private ObservableCollection<Models.ChangePW> _AllChPwRs = new ObservableCollection<Models.ChangePW>();
    public ObservableCollection<Models.ChangePW> AllChPwRs
    {
        get
        {
            return _AllChPwRs;
        }
        set
        {
            SetProperty(ref _AllChPwRs, value);
            if (value != null)
                MyMenu.Where(c => c.ID == 9).FirstOrDefault().Badge = (value.Count() > 0 )? value.ToString() : null;
        }
    }*/
    private ObservableCollection<Models.Feedback> _MyFBs = new ObservableCollection<Models.Feedback>();
    public ObservableCollection<Models.Feedback> MyFBs
    {
        get
        {
            return _MyFBs;
        }
        set
        {
            if(SetProperty(ref _MyFBs, value))
            {
                OnPropertyChanged(nameof(Isfbmsg));
            }
        }
    }


    public void OnAppering(){
        /*bool rv = false;
        int t = 0;
        if (AllCartProducts.Count > 0)
        {
            foreach (CartProducts product in AllCartProducts)
            {
                t += product.Count;
            }
            if(t>0)
                rv = true;
        }
        else
            rv = false;
        ShoppingCartBage = GlobalFunc.ConvertNumberToAr(t.ToString());
        IsShoppingCart = rv;
        OnPropertyChanged(nameof(ShoppingCartBage));
        OnPropertyChanged(nameof(IsShoppingCart));
        if (IsAdmin)
        {
            if (Consts.NotActivatedUsers != null)
                MyMenu.Where(c => c.ID == 6).FirstOrDefault().Badge = (Consts.NotActivatedUsers.Count() > 0) ? Consts.NotActivatedUsers.Count().ToString() : null;
            if (Consts.CanceledActivatedUsers != null)
                MyMenu.Where(c => c.ID == 7).FirstOrDefault().Badge = (Consts.CanceledActivatedUsers.Count() > 0) ? Consts.CanceledActivatedUsers.Count().ToString() : null;
        }
        */
        OnPropertyChanged(nameof(Isfbmsg));
    }


    public Command NavigateToDetailPageCommand { get; }
    public Command RefreshCommand { get; }
    public Command OnMessageButtonClickedCommand { get; }
    public Command OpenSearchBarCommand { get; }
    public Command PerformSearchCommand { get; }


    public async Task<ObservableCollection<Teacher>> GetTeachersAsync()
    {
        var json = await GlobalFunc.APIGetTeachers();
        if (json != "error01")
        {
            //await App.Current.MainPage.DisplayAlert("test (1)", json, "OK");
            var pu = JsonConvert.DeserializeObject<ObservableCollection<Teacher>>(json);
            return pu;
        }
        else
        {
            //return null;
            return new ObservableCollection<Teacher>();
        }
    }
    public async Task<ObservableCollection<Models.Feedback>> AdminGetAllFBsAsync()
    {
        var json = await GlobalFunc.Admin_APIAllFeedbacks(AL_HomePage.CU.UserID);
        //await PopupNavigation.Instance.PushAsync(new Msg(json, "test2", Microsoft.Maui.Graphics.Color.FromHex("#ff3b2f"), "موافق"));
        if (json.Substring(0, 5) != "error")
        {
            var pu = JsonConvert.DeserializeObject<ObservableCollection<Models.Feedback>>(json);
            return pu;
        }
        else
        {
            //return null;
            return new ObservableCollection<Models.Feedback>();
        }
    }

    public async Task<ObservableCollection<Models.Feedback>> GetMyFBsAsync()
    {
        var json = await GlobalFunc.APIGetMyFeedbacks(AL_HomePage.CU.UserID);
        if (json.Substring(0, 5) != "error")
        {
            var pu = JsonConvert.DeserializeObject<ObservableCollection<Models.Feedback>>(json);
            return pu;
        }
        else
        {
            //return null;
            return new ObservableCollection<Models.Feedback>();
        }
    }
    public async Task<Models.User> KeepMeLoggedInNewDataAsync()
    {
        var json = await GlobalFunc.UpdateUserData(AL_HomePage.CU.UserID);
        if (json.Substring(0, 5) != "error")
        {
            try
            {
                var pu = JsonConvert.DeserializeObject<Models.User>(json);
                return pu;
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return null;
            //return new Models.User();
        }
    }

    PopUpLoadingViewModel _PopUpLoading;
    int step = 0, done = 0;
    private async Task ExeccuteRefreshCommand()
    {
        if (_PopUpLoading != null && _PopUpLoading.IsLoading)
            return;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await MsgWithIcon.ShowNoConn(Navigation);
            return;
        }
        var tokenSource2 = new CancellationTokenSource();
        CancellationToken ct = tokenSource2.Token;
        if (!firststart)
        {
            var task = Task.Run(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();
                while (!ct.IsCancellationRequested)
                {
                    if (RefreshImgRotation >= 360f) RefreshImgRotation = 0;
                    RefreshImgRotation += 1;
                    Task.Delay(100);
                }
                RefreshImgRotation = 0;
                ct.ThrowIfCancellationRequested();
            }, tokenSource2.Token);
        }

        if (step == 0)
        {
            _PopUpLoading = new PopUpLoadingViewModel(Navigation);
            await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));
        }
        if (step == 0)
        {
            try
            {
                AllTeachers = Teachers = await GetTeachersAsync();

                IsEmpty = AllTeachers.Count() == 0;
                step = 1;
            }
            catch (Exception e)
            {
                if (done == 0)
                {
                    done++;
                    await ExeccuteRefreshCommand();
                }
                else
                {
                    _PopUpLoading.Done();
                    await MsgWithIcon.ShowNoConn(Navigation);
                    done = step = 0;
                }
                //await App.Current.MainPage.DisplayAlert("test (1)", e.StackTrace, "OK");
            }
        }
        if (step > 0)
        {

            if (IsAdmin)
            {
                try
                {
                    //await App.Current.MainPage.DisplayAlert("test (1)","", "OK");
                    //Consts.PColors = await GetColorsAsync();
                    //await App.Current.MainPage.DisplayAlert("test (2)", "", "OK");
                    //Consts.NotActivatedUsers = await GetNotActivatedusersAsync(0);

                    //await App.Current.MainPage.DisplayAlert("test (3)", "", "OK");
                    //if (Consts.NotActivatedUsers != null)
                    //    MyMenu.Where(c => c.ID == 6).FirstOrDefault().Badge = (Consts.NotActivatedUsers.Count() > 0) ? Consts.NotActivatedUsers.Count().ToString() : null;

                    //await App.Current.MainPage.DisplayAlert("test (4)", "", "OK");
                    //Consts.CanceledActivatedUsers = await GetNotActivatedusersAsync(2);
                    //if (Consts.CanceledActivatedUsers != null)
                    //   MyMenu.Where(c => c.ID == 7).FirstOrDefault().Badge = (Consts.CanceledActivatedUsers.Count() > 0) ? Consts.CanceledActivatedUsers.Count().ToString() : null;
                    //await App.Current.MainPage.DisplayAlert("test (5)", "", "OK");
                    AllFBs = await AdminGetAllFBsAsync();
                    //await App.Current.MainPage.DisplayAlert("test (6)", "", "OK");
                    //AllChPwRs = await AdminGetAllChPwRAsync();
                    //await App.Current.MainPage.DisplayAlert("test (7)", "", "OK");
                    //Admin_AllOrders = await AdminGetAllOrdersAsync();
                    //await App.Current.MainPage.DisplayAlert("test (8)", "", "OK");
                    /*if (Admin_AllOrders.Where(c => c.OrderStatus == 0).Any())
                    {
                        var temp = new ObservableCollection<Models.Order>();
                        foreach( var i in Admin_AllOrders.Where((i) => i.OrderStatus == 0))
                        {
                            temp.Add(i);
                        }
                        Admin_AllPendingOrders = temp;
                        //OnPropertyChanged(nameof(Admin_AllPendingOrders));
                    }*/
                    step = 2;
                }
                catch (Exception e)
                {
                    //await App.Current.MainPage.DisplayAlert("test (1)", e.StackTrace, "OK");
                    if (done == 0)
                    {
                        done++;
                        await ExeccuteRefreshCommand();
                    }
                    else
                    {
                        _PopUpLoading.Done();
                        await MsgWithIcon.ShowNoConn(Navigation);
                        done = step = 0;
                    }
                    //await App.Current.MainPage.DisplayAlert("test (2)", e.StackTrace, "OK");
                }
            }
            //await App.Current.MainPage.DisplayAlert("test (1)", "done1\n" + MyFBs.Count(), "OK");
            if (!IsAdmin)
            {
                //await App.Current.MainPage.DisplayAlert("test (1)", "done", "OK");
                //if(AL_HomePage.CU.UserType == 2)
                //{
                try
                {
                    //await App.Current.MainPage.DisplayAlert("test (2)", "done", "OK");

                    var c = await KeepMeLoggedInNewDataAsync();
                    if (c != null)
                    {
                        //await App.Current.MainPage.DisplayAlert("test (3)", "done", "OK");
                        AL_HomePage.CU = c;
                        //await App.Current.MainPage.DisplayAlert("test (4)", "done", "OK");

                    }
                }
                catch (Exception e)
                {
                    if (done == 0)
                    {
                        done++;
                        await ExeccuteRefreshCommand();
                    }
                    else
                    {
                        _PopUpLoading.Done();
                        await MsgWithIcon.ShowNoConn(Navigation);
                        done = step = 0;
                    }
                }
                //}

                try
                {
                    MyFBs = await GetMyFBsAsync();
                    OnPropertyChanged(nameof(Isfbmsg));
                    //MyOrders = await GetMyOrdersAsync();
                    step = 3;
                    //await App.Current.MainPage.DisplayAlert("test (3)", "done\n"+MyFBs.Count(), "OK");
                }
                catch (Exception e)
                {
                    //await App.Current.MainPage.DisplayAlert("test (3)", e.StackTrace, "OK");
                    if (done == 0)
                    {
                        done++;
                        await ExeccuteRefreshCommand();
                    }
                    else
                    {
                        _PopUpLoading.Done();
                        await MsgWithIcon.ShowNoConn(Navigation);
                        done = step = 0;
                    }
                }
            }
        }
        /*if (step > 1)
        {

            try
            {
                IsSubCat = false;
                int emptytemp = 0;
                foreach (Category c in Categories)
                {
                    emptytemp += c.numberItems;
                }
                IsEmpty = emptytemp == 0;
                AllCartProducts = new ObservableCollection<CartProducts>();
                IsShoppingCart = false;
                OnPropertyChanged(nameof(IsShoppingCart));
                _PopUpLoading.Done();
                step = 3;
            }
            catch (Exception e)
            {
                if (done == 0)
                {
                    done++;
                    await ExeccuteRefreshCommand();
                }
                else
                {
                    _PopUpLoading.Done();
                    await MsgWithIcon.ShowNoConn(Navigation);
                    done = step = 0;
                }
                //await App.Current.MainPage.DisplayAlert("test (4)", e.StackTrace, "OK");
            }
        }*/



        _PopUpLoading.Done();
        if (!firststart)
        {
            tokenSource2.Cancel();
        }
        firststart = false;
        done = step = 0;
    }
    private async Task ExeccuteNavigateToDetailPageCommand(Teacher teacher)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            if (IsGuest)
            {
                if (await ConfirmMsg.ConfirmMessage("من فضلك قم بتسجيل الدخول لتتمكن من الأستفاده الكاملة من البرنامج.", "تأكيد:", "ليس الأن", "تسجيل الدخول", Navigation))
                {
                    await AppShell.GoToPage(nameof(SigninPage));
                    //await Navigation.PushAsync(new SigninPage());
                }
                return;
            }
            if (teacher.IsPublic)
            {
               /* var parameters = new ShellNavigationQueryParameters
                {
                    { "teacherJson", JsonConvert.SerializeObject(teacher) },
                };
                await AppShell.GoToPage(nameof(AL_TeacherView), parameters);*/
                await Navigation.PushAsync(new AL_TeacherView(teacher));
            }
            else
            {
                string rv = await GlobalFunc.CheckAccessTeachers(AL_HomePage.CU.UserID.ToString(), teacher.UserID.ToString());
                if (rv == "error300" || rv == "error301")
                {
                    GlobalFunc.ToastShow("لقد انتهت صلاحية جلسة تسجيل الدخول الخاصة بك، يرجى إعادة تسجيل الدخول");
                    AL_HomePage.CU = null;
                    await AppShell.GoToPage(nameof(SigninPage));
                    //await Navigation.PushAsync(new HomePage());
                    return;
                }
                if (rv != "succ")
                {
                    var code = await GetEntryText.GetEntryTxt("من فضلك ادخل كود المحاضر", "كود المحاضر", 500, 1, "الغاء", "موافق", Navigation);
                    if (code != "c")
                    {
                        if (string.IsNullOrEmpty(code) || code.Length < 3)
                            return;
                        string rv2 = await GlobalFunc.CheckAccessTeachers(AL_HomePage.CU.UserID.ToString(), teacher.UserID.ToString(), c: code);
                        if (rv2 == "succ")
                        {
                            /*var parameters = new ShellNavigationQueryParameters
                            {
                                { "teacherJson", JsonConvert.SerializeObject(teacher) },
                            };
                            AppShell.GoToPage(nameof(AL_TeacherView), parameters);*/
                            await Navigation.PushAsync(new AL_TeacherView(teacher));
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
                else
                {
                    /*var parameters = new ShellNavigationQueryParameters
                            {
                                { "teacherJson", JsonConvert.SerializeObject(teacher) },
                            };
                    AppShell.GoToPage(nameof(AL_TeacherView), parameters);*/
                    await Navigation.PushAsync(new AL_TeacherView(teacher));
                }
            }
        }, Navigation);
    }

}
