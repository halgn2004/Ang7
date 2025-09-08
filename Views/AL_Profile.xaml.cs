using Ang7.Models;
using Ang7.ViewModels;
using Ang7.Views.PopUp;
using Newtonsoft.Json;
using RGPopup.Maui.Services;
using System.Collections.ObjectModel;

namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AL_Profile : ContentPage
{
    AL_ProfileViewModel vm;
    public AL_Profile()
    {
        InitializeComponent();
        BindingContext = vm = new AL_ProfileViewModel(Navigation);
    }

    private void Onback_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        if (PopupNavigation.Instance.PopupStack.Any())
        {
            // Close the current popup
            TapEventHandler.Reset_isNavigating();
            PopupNavigation.Instance.PopAsync();
            return true; // Return true to prevent the back press from affecting the main page navigation
        }

        //Device.BeginInvokeOnMainThread(() => {
            Navigation.PopAsync();
        //});
        return true;
    }
    private async void OnCHPWTapped(object sender, TappedEventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            await PopupNavigation.Instance.PushAsync(new PopUp.ChangePW(new PopUpChangePWViewModel(Navigation)));
        }, Navigation);

    }
    private async void OnNameTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true,async () =>
        {
            var newname = await GetEntryText.GetEntryTxt("تغيير الأسم", "أسم", 100, 1, "تراجع", "موافق", Navigation);
            if (newname != "c")
            {
                if (newname == AL_HomePage.CU.Name)
                    await PopupNavigation.Instance.PushAsync(new Msg("لم يحدث اي تغير في الأسم الخاص بك.", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (newname.Length < 3)
                    await PopupNavigation.Instance.PushAsync(new Msg("الأسم قصير جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (newname.Length > 50)
                    await PopupNavigation.Instance.PushAsync(new Msg("الأسم طويل جدا", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (int.TryParse(newname, out CheckIfNymbersOnly))
                    await PopupNavigation.Instance.PushAsync(new Msg("الأسم لا يمكن ان يتكون من أرقام فقط", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else
                {
                    _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 1, newname);
                    AL_HomePage.CU.Name = newname;
                    vm.User = AL_HomePage.CU;
                }
            }
        }, Navigation);
    }
    private async void OnTSTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true,async () =>
        {
            var newPhone = await GetEntryText.GetEntryTxt("تغيير رقم الهاتف", "رقم هاتفك", 11, 2, "تراجع", "موافق", Navigation);
            if (newPhone != "c")
            {
                if (newPhone == AL_HomePage.CU.Phone)
                    await PopupNavigation.Instance.PushAsync(new Msg("لم يحدث اي تغير في رقم الهاتف الخاص بك.", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (newPhone.Length != 11 || newPhone.Substring(0, 2) != "01")
                    await PopupNavigation.Instance.PushAsync(new Msg("رقم الهاتف غير صحيح", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else
                {
                    _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 2, newPhone);
                    AL_HomePage.CU.Phone = newPhone;
                    vm.User = AL_HomePage.CU;
                }
            }
        },Navigation);
    }
    PopUpLoadingViewModel _PopUpLoading;
    string fn;//= AL_HomePage.CU.PP;
    public Stream streamtosend;
    private int CheckIfNymbersOnly;

    private async void OnPPTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            fn = AL_HomePage.CU.PP;
            var li = new ObservableCollection<PopUpSelectItemModel> {
        new PopUpSelectItemModel{Img = "popupselect_01.png", Desc = "أختر صوره رمزية"},
        new PopUpSelectItemModel{Img = "popupselect_02.png", Desc = "الكاميرا"},
        new PopUpSelectItemModel{Img = "popupselect_03.png", Desc = "الصور"}
    };

            var act = await Select.SelectMessage("أستبدل الصوره", li, !(AL_HomePage.CU.PP.Contains("nouserpic.png")), Navigation);
            FileResult photo = null;

            if (act == li[0].Desc)
            {
                string sa = await SelectAvatar.SAvatar(Navigation);
                if (sa != "cancel")
                {
                    fn = sa;
                    streamtosend = null;
                    photo = null;
                }
            }
            else if (act == li[1].Desc)
            {
                var mediaOptions = new MediaPickerOptions
                {
                    Title = "Take Photo"
                };

                try
                {

                    // Ensure no pending navigation before launching the camera
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam);
#endif
                    photo = await MediaPicker.Default.CapturePhotoAsync(mediaOptions);
                    if (photo != null)
                    {
                        //fn = $"{Guid.NewGuid()}.jpg";
                        fn = $"{Guid.NewGuid()}.{Path.GetExtension(photo.FullPath)}";
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    // Handle no camera support
                    return;
                }
                catch (PermissionException)
                {
                    // Handle permission issues
                    return;
                }
                catch (Exception ex)
                {
                    // Handle other errors
                    return;
                }
            }
            else if (act == li[2].Desc)
            {
                var mediaOptions = new MediaPickerOptions
                {
                    Title = "Pick a Photo"
                };

                try
                {
                    // Ensure no pending navigation before launching the camera
#if IOS
                    await Task.Delay(Consts.TaskDelayForIOSCam);
#endif
                    photo = await MediaPicker.Default.PickPhotoAsync(mediaOptions);
                    if (photo != null)
                    {
                        //fn = $"{Guid.NewGuid()}.jpg";
                        fn = $"{Guid.NewGuid()}.{Path.GetExtension(photo.FullPath)}";
                    }
                }
                catch (Exception ex)
                {
                    // Handle other errors
                    return;
                }
            }
            else if (act == "cancel")
            {
                streamtosend = null;
                photo = null;
                fn = AL_HomePage.CU.PP;
            }
            else if (act == "delete")
            {
                streamtosend = null;
                photo = null;
                fn = "nouserpic.png";
            }

            if (fn == AL_HomePage.CU.PP)
            {
                return;
            }
            else
            {
                _PopUpLoading = new PopUpLoadingViewModel(Navigation);
                await PopupNavigation.Instance.PushAsync(new Loading(_PopUpLoading));

                if (photo == null)
                {
                    streamtosend = null;
                }
                else
                {
                    using var stream = await photo.OpenReadAsync();
                    fn = photo.FullPath;

                    try
                    {
                        var l = await GlobalFunc.UploadAttachment(stream, fn, 1);
                        var jr = JsonConvert.DeserializeObject<UploadFileAPIResponse>(l);
                        fn = Consts.APIUrl + "uploaded_files/pp/" + jr.FILENAME;
                        streamtosend = null;
                    }
                    catch (Exception ex)
                    {
                        _PopUpLoading.ShowMsg(ex.Message, "OK");
                        return;
                    }
                }

                _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 3, fn);
                AL_HomePage.CU.PP = fn;
                vm.User = AL_HomePage.CU;
                _PopUpLoading.Done();
            }
        }, Navigation);

    }

    private async void OnDMATapped(object sender, TappedEventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true,async () =>
        {
            if (await ConfirmMsg.ConfirmMessage("هل أنت متأكد أنك تود ايقاف حسابك، سيتم حذف حسابك نهائيا بعد مرور 30 يوم بدون اعاده تفعيل؟", "تأكيد:", "لا", "نعم", Navigation))
            {
                var pw = await GetEntryText.GetEntryTxt("من فضلك ادخل كلمة المرور الخاصه بحسابك", "كلمة المرور", 500, 1, "تراجع", "موافق", Navigation);
                if (pw != "c")
                {
                    if (string.IsNullOrEmpty(pw) || pw.Length < 6)
                    {
                        await PopupNavigation.Instance.PushAsync(new Msg("كلمة مرور غير صحيحة", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                    }
                    else
                    {
                        var json = await GlobalFunc.APIDelAcc(AL_HomePage.CU.UserID, pw);
                        if (json == "Done.")
                        {
                            AL_HomePage.CU = null;
                            await AppShell.GoToPage(nameof(HomePage));
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Msg("كلمة مرور غير صحيحة", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        }
                    }
                }
            }
        },Navigation);

    }
    private async void OnDMAFTapped(object sender, TappedEventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true,async () =>
        {
            if (await ConfirmMsg.ConfirmMessage("هل أنت متأكد أنك تود حذف حسابك نهائيا، سيتم حذف جميع مايخصك وهذه الخطوه لايمكن العوده بها؟", "تأكيد:", "لا", "نعم", Navigation))
            {
                var pw = await GetEntryText.GetEntryTxt("من فضلك ادخل كلمة المرور الخاصه بحسابك", "كلمة المرور", 500, 1, "تراجع", "موافق", Navigation);
                if (pw != "c")
                {
                    if (string.IsNullOrEmpty(pw) || pw.Length < 6)
                    {
                        await PopupNavigation.Instance.PushAsync(new Msg("كلمة مرور غير صحيحة", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                    }
                    else
                    {
                        var json = await GlobalFunc.APIFinalDelAcc(AL_HomePage.CU.UserID, pw);
                        if (json == "Done.")
                        {
                            AL_HomePage.CU = null;
                            await AppShell.GoToPage(nameof(HomePage));
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Msg("كلمة مرور غير صحيحة", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                        }
                    }
                }
            }
        },Navigation);

    }

    private async void OnBIOTapped(object sender, EventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true,async () =>
        {
            var newbio = await GetEditorText.GetEditorTxt("تغيير السيره الذاتية", "السيره الذاتية", 500, 1, "تراجع", "موافق", Navigation);
            if (newbio != "c")
            {
                if (newbio == AL_HomePage.CU.Bio)
                    await PopupNavigation.Instance.PushAsync(new Msg("لم يحدث اي تغير في السيره الذاتية الخاصه بك.", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else if (int.TryParse(newbio, out CheckIfNymbersOnly))
                    await PopupNavigation.Instance.PushAsync(new Msg("السيره الذاتية لا يمكن ان تتكون من أرقام فقط", "خطأ", Microsoft.Maui.Graphics.Color.FromArgb("#ff3b2f"), "موافق"));
                else
                {
                    _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 6, newbio);
                    AL_HomePage.CU.Bio = newbio;
                    vm.User = AL_HomePage.CU;
                    //IsNameChanged = true;
                }
            }
        },Navigation);
    }

    private async void OnSubjTapped(object sender, TappedEventArgs e)
    {
        await TapEventHandler.HandleTapEvent(true, async () =>
        {
            var li = new ObservableCollection<PopUpSelectItemModel> {
                new PopUpSelectItemModel{Img = "popupselect_10.png" , Desc = "أضافة ماده جديده"},
                new PopUpSelectItemModel{Img = "popupselect_11.png" , Desc = "حذف ماده حالية"},
                new PopUpSelectItemModel{Img = "popupselect_12.png" , Desc = "تعديل مادة حالية"}
            };
            var act = await Select.SelectMessage("تعديل المواد", li, false, Navigation);
            if (act == li[0].Desc)
            {
                var newname = await GetEntryText.GetEntryTxt("أضافة ماده جديده", "أسم المادة", 100, 1, "تراجع", "موافق", Navigation);
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
                        _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 7, newname);
                        AL_HomePage.CU.Subjects.Add(newname);
                        vm.User = AL_HomePage.CU;
                    }
                }
            }
            else if (act == li[1].Desc)
            {
                string prefix = "حذف ";
                var li2 = CreateSubjectsoptions(prefix, AL_HomePage.CU.Subjects, "popupselect_20.png");
                var act2 = await Select.SelectMessage("أختر الماده", li2, false, Navigation);
                string sub = act2.Replace(prefix, "");
                if (AL_HomePage.CU.Subjects.Contains(sub))
                {
                    _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 8, sub);
                    AL_HomePage.CU.Subjects.Remove(sub);
                    vm.User = AL_HomePage.CU;
                }
            }
            else if (act == li[2].Desc)
            {
                string prefix = "تعديل ";
                var li2 = CreateSubjectsoptions(prefix, AL_HomePage.CU.Subjects, "popupselect_21.png");
                var act2 = await Select.SelectMessage("أختر الماده", li2, false, Navigation);
                string sub = act2.Replace(prefix, "");
                if (AL_HomePage.CU.Subjects.Contains(sub))
                {
                    var newname = await GetEntryText.GetEntryTxt("تعديل مادة " + sub, "أسم المادة الجديد", 100, 1, "تراجع", "موافق", Navigation);
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
                            _ = await GlobalFunc.UpdateUser(AL_HomePage.CU.UserID, 9, newname, ov: sub);
                            int index = AL_HomePage.CU.Subjects.IndexOf(sub);
                            AL_HomePage.CU.Subjects[index] = newname;
                            vm.User = AL_HomePage.CU;
                        }
                    }
                }
            }

        }, Navigation);
    }

    private ObservableCollection<PopUpSelectItemModel> CreateSubjectsoptions(string prefix , List<string> sj ,string img)
    {
        ObservableCollection<PopUpSelectItemModel> rv = new ObservableCollection<PopUpSelectItemModel>();
        foreach(string s in sj)
        {
            rv.Add(new PopUpSelectItemModel { Img = $"{img}", Desc = prefix + s });
        }
        return rv;

    }

}