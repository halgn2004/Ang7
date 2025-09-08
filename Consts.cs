using Ang7.Models;
using Ang7.Views;
using Ang7.Views.PopUp;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using PdfSharpCore.Fonts;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace Ang7;

public static class Consts
{
    public static int TaskDelayForIOSCam = 500;

    //public static string APIUrl = "https://ang7.onmediawll.com/API/";//"http://www.onmediawll.com/Anga7/API/";
    public static string APIUrl = "https://anga7.com/API/";
    public static string GMEmail = "ang7@onmediawll.com";

    /*public const string androidPackageName = "com.onmediawll.ang7";
    public const string iOSApplicationId = "6738999691";//ULC59MPPVW.com.onmediawll.ang7
    public const string windowsProductId = "9nblggh5l9xt";*/
    public const string androidPackageName = "com.onmediawll.ang7";
    public const string iOSApplicationId = "id6738999691";
    public const string AppStoreUrl = $"https://apps.apple.com/app/{iOSApplicationId}";   // iOS Store link
    public const string PlayStoreUrl = $"https://play.google.com/store/apps/details?id={androidPackageName}"; // Android Store link

    public const int OrderIncreassedBy = 1000;
    public static string[] fbstitle = { "مقترح", "أستفسار", "شكوي", "مشكلة" }; 
    public static string AppVersion = GlobalFunc.ConvertNumberToAr(VersionTracking.CurrentVersion);

    public static string PrivacyPolicy_HTML = "<!DOCTYPE html><html dir='rtl' lang='ar'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width'><title>سياسة الخصوصية</title><style> body { font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; padding:1em; } </style></head><body><strong>سياسة الخصوصية</strong> <p>قام المطور ببناء التطبيق كتطبيق مجاني. يتم توفير هذه الخدمة بدون تكلفة وهي مخصصة للاستخدام كما هي.</p> <p>تُستخدم هذه الصفحة لإعلام الزائرين بشأن سياساتي من خلال جمع المعلومات الشخصية واستخدامها والكشف عنها إذا قرر أي شخص استخدام خدمتي. </ p> <p> إذا اخترت استخدام خدمتي ، فأنت توافق على الجمع واستخدام المعلومات المتعلقة بهذه السياسة. يتم استخدام المعلومات الشخصية التي أجمعها لتوفير الخدمة وتحسينها. لن أستخدم أو أشارك معلوماتك مع أي شخص باستثناء ما هو موضح في سياسة الخصوصية هذه.</p> <p>المصطلحات المستخدمة في سياسة الخصوصية هذه لها نفس المعاني الواردة في الشروط والأحكام الخاصة بنا ، والتي يمكن الوصول إليها في التطبيق ما لم يتم تحديد خلاف ذلك في سياسة الخصوصية هذه. </p> <p><strong>جمع المعلومات واستخدامها</strong></p> <p>للحصول على تجربة أفضل ، أثناء استخدام خدمتنا ، قد أطلب منك تزويدنا بمعلومات تعريف شخصية معينة. سيتم الاحتفاظ بالمعلومات التي أطلبها على جهازك ولن يتم جمعها بأي شكل من الأشكال. </ p> <div> <p> يستخدم التطبيق خدمات الجهات الخارجية التي قد تجمع المعلومات المستخدمة لتحديد هويتك. </ p > <p> رابط إلى سياسة الخصوصية لموفري خدمات الجهات الخارجية التي يستخدمها التطبيق</p> <ul><li><a href=\"https://www.google.com/policies/privacy/\" target=\"_blank\" rel=\"noopener noreferrer\">خدمات جوجل بلاي</a></li><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----></ul></div> <p><strong>تسجيل البيانات</strong></p> <p>أريد إبلاغك أنه كلما استخدمت خدمتي ، في حالة حدوث خطأ في التطبيق ، أقوم بجمع البيانات والمعلومات (من خلال منتجات تابعة لجهات خارجية) على هاتفك تسمى تسجيل البيانات. قد تتضمن بيانات السجل هذه معلومات مثل عنوان بروتوكول الإنترنت (“IP”) الخاص بجهازك ، واسم الجهاز ، وإصدار نظام التشغيل ، وتكوين التطبيق عند استخدام خدمتي ، ووقت وتاريخ استخدامك للخدمة ، وإحصاءات أخرى . </p> <p> <strong> ملفات تعريف الارتباط </strong> </p> <p> ملفات تعريف الارتباط هي ملفات تحتوي على كمية صغيرة من البيانات تُستخدم بشكل شائع كمعرفات فريدة مجهولة الهوية. يتم إرسالها إلى متصفحك من مواقع الويب التي تزورها ويتم تخزينها على الذاكرة الداخلية لجهازك.</p> <p>لا تستخدم هذه الخدمة “ملفات تعريف الارتباط” بشكل صريح. ومع ذلك ، قد يستخدم التطبيق رموزًا ومكتبات خاصة بطرف ثالث تستخدم “ملفات تعريف الارتباط” لجمع المعلومات وتحسين خدماتهم. لديك خيار إما قبول أو رفض ملفات تعريف الارتباط هذه ومعرفة متى يتم إرسال ملف تعريف الارتباط إلى جهازك. إذا اخترت رفض ملفات تعريف الارتباط الخاصة بنا ، فقد لا تتمكن من استخدام بعض أجزاء هذه الخدمة.</p> <p><strong>مقدمي الخدمة</strong></p> <p>يجوز لي توظيف شركات وأفراد تابعين لجهات خارجية للأسباب التالية: </ p> <ul> <li> لتسهيل خدمتنا ؛ </li> <li> لتقديم الخدمة نيابة عنا ؛ </li> <li> لأداء الخدمات المتعلقة بالخدمة ؛ أو </li> <li> لمساعدتنا في تحليل كيفية استخدام خدمتنا. </li> </ul> <p> أريد إبلاغ مستخدمي هذه الخدمة بأن هذه الأطراف الثالثة لديها حق الوصول إلى معلوماتك الشخصية. والسبب هو أداء المهام الموكلة إليهم نيابة عنا. ومع ذلك ، فهم ملزمون بعدم الكشف عن المعلومات أو استخدامها لأي غرض آخر.</p> <p><strong>الأمان</strong></p> <p>أنا أقدر ثقتك في تزويدنا بمعلوماتك الشخصية ، وبالتالي فإننا نسعى جاهدين لاستخدام وسائل مقبولة تجاريًا لحمايتها. لكن تذكر أنه لا توجد وسيلة نقل عبر الإنترنت أو طريقة تخزين إلكتروني آمنة وموثوقة بنسبة 100٪ ، ولا يمكنني ضمان أمانها المطلق. </ p> <p> <strong> روابط إلى مواقع أخرى</strong></p> <p>قد تحتوي هذه الخدمة على روابط لمواقع أخرى. إذا قمت بالنقر فوق ارتباط جهة خارجية ، فسيتم توجيهك إلى هذا الموقع. لاحظ أن هذه المواقع الخارجية لا يتم تشغيلها بواسطتي. لذلك ، أنصحك بشدة بمراجعة سياسة الخصوصية الخاصة بهذه المواقع. ليس لدي أي سيطرة ولا أتحمل أي مسؤولية عن المحتوى أو سياسات الخصوصية أو الممارسات الخاصة بأي مواقع أو خدمات تابعة لجهات خارجية. </ p> <p> <strong> خصوصية الأطفال </strong> </p> <p> لا تخاطب هذه الخدمات أي شخص يقل عمره عن 13 عامًا. أنا لا أجمع عن قصد معلومات تعريف شخصية من الأطفال دون سن 13 عامًا. في حالة اكتشاف أن طفلاً أقل من 13 عامًا قد زودني بمعلومات شخصية ، فأنا أحذفها على الفور من خوادمنا. إذا كنت والدًا أو وصيًا وكنت على علم بأن طفلك قد زودنا بمعلومات شخصية ، فيرجى الاتصال بي حتى أتمكن من القيام بالإجراءات اللازمة.</p> <p><strong>التغييرات على سياسة الخصوصية</strong></p> <p>قد أقوم بتحديث سياسة الخصوصية الخاصة بنا من وقت لآخر. وبالتالي ، يُنصح بمراجعة هذه الصفحة بشكل دوري لمعرفة أي تغييرات. سأخطرك بأي تغييرات عن طريق نشر سياسة الخصوصية الجديدة على هذه الصفحة. </p> <p> هذه السياسة سارية اعتبارًا من 2022-07-01</p> <p><strong>اتصل بنا</strong></p> <p>إذا كان لديك أي أسئلة أو اقتراحات حول سياسة الخصوصية الخاصة بي ، فلا تتردد في الاتصال بي على "+ GMEmail + ".</p> </p></body></html>";
    public static string TermsOfService_HTML = "<!DOCTYPE html><html dir='rtl' lang='ar'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width'><title>الأحكام والشروط</title><style> body { font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; padding:1em; } </style></head> <body><strong>الأحكام والشروط</strong> <p>عن طريق تنزيل التطبيق أو استخدامه ، ستنطبق هذه الشروط تلقائيًا عليك - لذلك يجب عليك التأكد من قراءتها بعناية قبل استخدام التطبيق. لا يُسمح لك بنسخ أو تعديل التطبيق أو أي جزء من التطبيق أو علاماتنا التجارية بأي شكل من الأشكال. لا يُسمح لك بمحاولة استخراج شفرة المصدر للتطبيق ، ولا يجب أيضًا محاولة ترجمة التطبيق إلى لغات أخرى ، أو إنشاء إصدارات مشتقة. التطبيق نفسه وجميع العلامات التجارية وحقوق التأليف والنشر وحقوق قاعدة البيانات وحقوق الملكية الفكرية الأخرى المتعلقة به ، لا تزال ملكًا ل فالسوق.</p> <p>يلتزم المطور بضمان أن يكون التطبيق مفيدًا وفعالًا قدر الإمكان. لهذا السبب ، نحتفظ بالحق في إجراء تغييرات على التطبيق أو فرض رسوم على خدماته في أي وقت ولأي سبب. لن نفرض عليك رسومًا مقابل التطبيق أو خدماته أبدًا دون توضيح ما تدفع مقابله بالضبط.</p> <p>يخزن تطبيق فالسوق ويعالج البيانات الشخصية التي قدمتها إلينا من أجل تقديم الخدمة الخاصة بي. تقع على عاتقك مسؤولية الحفاظ على أمان هاتفك والوصول إلى التطبيق. لذلك نوصيك بعدم كسر الحماية أو عمل روت لهاتفك ، وهي عملية إزالة قيود البرامج والقيود التي يفرضها نظام التشغيل الرسمي لجهازك. يمكن أن يجعل هاتفك عرضة للبرامج الضارة / الفيروسات / البرامج الضارة ، ويهدد ميزات أمان هاتفك وقد يعني أن تطبيق فالسوق لن يعمل بشكل صحيح أو على الإطلاق.</p> <div><p>لا يستخدم التطبيق خدمات الجهات الخارجية التي تعلن عن البنود والشروط الخاصة بها.</p> <p>رابط لشروط وأحكام مزودي خدمة الطرف الثالث الذين يستخدمهم التطبيق </p> <ul><li><a href=\"https://policies.google.com/terms\" target=\"_blank\" rel=\"noopener noreferrer\">خدمات جوجل بلاي</a></li><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----><!----></ul></div> <p>يجب أن تدرك أن هناك أشياء معينة لن تتحمل فالسوق مسؤوليتها. تتطلب بعض وظائف التطبيق أن يكون للتطبيق اتصال إنترنت نشط. يمكن أن يكون الاتصال عبر وايفاي ، أو مقدمًا من مزود شبكة الهاتف المحمول الخاص بك ، ولكن لا يمكن لـ فالسوق تحمل مسؤولية التطبيق الذي لا يعمل بكامل وظائفه إذا لم يكن لديك وصول إلى شبكة وايفاي ، ولم يكن لديك أي من ترك بدل البيانات. </p> <p></p> <p>إذا كنت تستخدم التطبيق خارج منطقة مزودة بشبكة وايفاي ، فيجب أن تتذكر أن شروط الاتفاقية مع مزود شبكة الجوال الخاص بك ستظل سارية. نتيجة لذلك ، قد يتم تحميلك من قبل مزود خدمة الهاتف المحمول الخاص بك مقابل تكلفة البيانات طوال مدة الاتصال أثناء الوصول إلى التطبيق ، أو رسوم الطرف الثالث الأخرى. عند استخدام التطبيق ، فأنت تقبل المسؤولية عن أي رسوم من هذا القبيل ، بما في ذلك رسوم بيانات التجوال إذا كنت تستخدم التطبيق خارج إقليمك الأصلي (أي المنطقة أو البلد) دون إيقاف تجوال البيانات. إذا لم تكن دافع الفاتورة للجهاز الذي تستخدم التطبيق عليه ، فيرجى العلم أننا نفترض أنك قد تلقيت إذنًا من دافع الفاتورة لاستخدام التطبيق.</p> <p> على نفس المنوال ، لا يمكن لـ فالسوق دائمًا تحمل مسؤولية الطريقة التي تستخدم بها التطبيق ، أي أنك تحتاج إلى التأكد من أن جهازك يظل مشحونًا - إذا نفدت البطارية ولا يمكنك تشغيله للاستفادة من الخدمة ، لا يمكن لـ فالسوق قبول المسؤولية. </ p> <p> فيما يتعلق بمسؤولية فالسوق عن استخدامك للتطبيق ، عند استخدام التطبيق ، من المهم أن تضع في اعتبارك أنه على الرغم من أننا نسعى لضمان تحديثه وصحته على الإطلاق في بعض الأحيان ، نعتمد على أطراف ثالثة لتزويدنا بالمعلومات حتى نتمكن من إتاحتها لك. لا تتحمل فالسوق أي مسؤولية عن أي خسارة ، مباشرة أو غير مباشرة ، تتعرض لها نتيجة الاعتماد كليًا على وظيفة التطبيق هذه.</p> <p>في مرحلة ما ، قد نرغب في تحديث التطبيق. التطبيق متاح حاليًا على أندرويد و أبل - قد تتغير متطلبات كلا النظامين (وأي أنظمة إضافية نقرر تمديد توفر التطبيق عليها) ، وستحتاج إلى تنزيل التحديثات إذا كنت تريد الاستمرار في استخدام التطبيق. لا تعد فالسوق بأنها ستقوم دائمًا بتحديث التطبيق بحيث يكون مناسبًا لك و / أو يعمل مع أندرويد ؛ إصدار أبل الذي قمت بتثبيته على جهازك. ومع ذلك ، فإنك تتعهد بقبول تحديثات التطبيق دائمًا عند عرضها عليك ، وقد نرغب أيضًا في التوقف عن توفير التطبيق ، وقد ننهي استخدامه في أي وقت دون إرسال إشعار بالإنهاء إليك. ما لم نخبرك بخلاف ذلك ، عند أي إنهاء ، (أ) تنتهي الحقوق والتراخيص الممنوحة لك في هذه الشروط ؛ (ب) يجب عليك التوقف عن استخدام التطبيق ، و (إذا لزم الأمر) حذفه من جهازك.</p> <p><strong>التغييرات على هذه الشروط والأحكام</strong></p> <p> قد أقوم بتحديث الشروط والأحكام الخاصة بنا من وقت لآخر. وبالتالي ، يُنصح بمراجعة هذه الصفحة بشكل دوري لمعرفة أي تغييرات. سأخطرك بأي تغييرات عن طريق نشر الشروط والأحكام الجديدة على هذه الصفحة. </p> <p> تسري هذه الشروط والأحكام اعتبارًا من 2022-06-01</p> <p><strong>اتصل بنا</strong></p> <p> إذا كان لديك أي أسئلة أو اقتراحات حول الشروط والأحكام الخاصة بي ، فلا تتردد في الاتصال بي على "+ GMEmail + ".</p> </p></body></html>";


    public static string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg", ".webp" };
    public static string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".flv", ".wmv", ".webm", ".mpeg" };
    public static string[] pdfExtensions = { ".pdf" };
}
public static class GlobalFunc
{
    public static async void ToastShow(string message , bool IsLong = true , int fontsize = 16)
    {
        var dur = IsLong ? ToastDuration.Long : ToastDuration.Short;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await Toast.Make(message,dur,fontsize).Show(cancellationTokenSource.Token);
    }
    public static int GetFileTypefromurl(string fileUrl)
    {
        // Extract file extension from URL
        string extension = System.IO.Path.GetExtension(fileUrl).ToLower();
        if (Consts.imageExtensions.Contains(extension))
        {
            return 2;
        }else if (Consts.videoExtensions.Contains(extension))
        {
            return 1;
        }else if (Consts.pdfExtensions.Contains(extension))
        {
            return 3;
        }else
            return -1;
    }
    public static string GetFileIcon(int i)
    {
        switch (i)
        {
            case -2:
                return "rb_full.png";
            case -1:
                return "folder48.png";
            case 1:
                return "video48.png";
            case 2:
                return "image48.png";
            case 3:
                return "pdf48.png";
            case 4:
                return "word48.png";
            case 5:
                return "excel48.png";
            case 6:
                return "powerPoint48.png";
            default:
                return "undefined48.png";
        }
    }
    public static async Task<bool> CheckPermissionsAndStart()
    {
#if !IOS
        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (cameraStatus != PermissionStatus.Granted)
        {
            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
        }
        // Request Storage permission
        var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        if (storageStatus != PermissionStatus.Granted)
        {
            storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
        }
        // For writing to storage (needed if saving files)
        var storageWriteStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        if (storageWriteStatus != PermissionStatus.Granted)
        {
            storageWriteStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        return (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted);
#else
        var PhotoStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();
        if (PhotoStatus != PermissionStatus.Granted)
        {
            PhotoStatus = await Permissions.RequestAsync<Permissions.Photos>();
        }
        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (cameraStatus != PermissionStatus.Granted)
        {
            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
        }
        var MicrophoneStatus = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (MicrophoneStatus != PermissionStatus.Granted)
        {
            MicrophoneStatus = await Permissions.RequestAsync<Permissions.Microphone>();
        }

        return (/*PhotoStatus == PermissionStatus.Granted && */cameraStatus == PermissionStatus.Granted && MicrophoneStatus == PermissionStatus.Granted);
#endif
    }


    public static async Task<string> APIDelFB(int uid, int fbid)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(fbid.ToString()), "\"fbid\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}DeleteFB.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APIGetAppVersion(int type = 1)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(type.ToString()), "\"type\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}appver.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }


    public static async Task RedirectToAppStore()
    {
        // Redirect based on the platform
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            await Launcher.OpenAsync(Consts.AppStoreUrl);
        }
        else if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            await Launcher.OpenAsync(Consts.PlayStoreUrl);
        }
    }
    public static async Task<string> Admin_UpdateActivated(int uid, int tuid, int tact)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(tuid.ToString()), "\"tuid\"" },
                { new StringContent(tact.ToString() == "0" ? "00" : tact.ToString()), "\"tact\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Admin_UpdateActivated.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> Admin_UpdatePW(int uid, int uppwui, int tact)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(uppwui.ToString()), "\"uppwui\"" },
                { new StringContent(tact.ToString() == "0" ? "00" : tact.ToString()), "\"tact\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Admin_UpdatePw.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APISendChangepwReq(string phone, string pw)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(phone), "\"uPhone\"" },
                { new StringContent(pw), "\"newPW\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}InsertCHangePWReq.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> Admin_SubmitCompleteFeedBack(int uid, int fbid,string reply)
    {

        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(fbid.ToString()), "\"fbid\"" },
                { new StringContent(string.IsNullOrEmpty(reply) ? "noreply" : reply), "\"reply\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Admin_SubmitFbr.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> Admin_GetNotActivatedUsers(int uid,int type)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(type.ToString() == "0" ? "00" : type.ToString()), "\"type\"" },
                { new StringContent(uid.ToString()), "\"uid\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Admin_GetNotActivatedUsers.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static string ConvertNumberToAr(string input)
    {
        var arabic = new string[10] { "۰", "۱", "۲", "۳", "٤", "٥", "٦", "٧", "۸", "۹" };

        for (int j = 0; j < arabic.Length; j++)
        {
            input = input.Replace(j.ToString(), arabic[j]);
        }
        return input;
    }
    public static string GetDeviceIdentifier()
    {
        var deviceId = DeviceInfo.Platform + "-" + DeviceInfo.Model + "-" + DeviceInfo.Manufacturer + "-" + DeviceInfo.Name;
        return deviceId;
    }

    public static string GetUniqueDeviceId()
    {
        var deviceId = DeviceInfo.Idiom.ToString() + "-" + DeviceInfo.DeviceType.ToString() + "-" + DeviceInfo.Name;
        return deviceId;
    }
    public static string ConvertDaysToAr(string input)
    {
        switch (input.ToLower().Substring(0, 3))
        {
            case "sat":
                return "السبت";
            case "sun":
                return "الأحد";
            case "mon":
                return "الأثنين";
            case "tue":
                return "الثلاثاء";
            case "wed":
                return "الأربعاء";
            case "thu":
                return "الخميس";
            case "fri":
                return "الجمعة";
            default:
                return "الجمعة";
        }
    }
    public static string ConvertmonthToAr(int input)
    {
        switch (input)
        {
            case 1:
                return "يناير";
            case 2:
                return "فبراير";
            case 3:
                return "مارس";
            case 4:
                return "إبريل";
            case 5:
                return "مايو";
            case 6:
                return "يونيو";
            case 7:
                return "يوليو";
            case 8:
                return "أغسطس";
            case 9:
                return "سبتمبر";
            case 10:
                return "أكتوبر";
            case 11:
                return "نوفمبر";
            case 12:
                return "ديسمبر";
            default:
                return "يناير";
        }
    }

    public static bool IsEmail(string s)
    {
        return Regex.Match(s, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success;
    }
    public static async Task<string> TestGettoken(string un, string pw)
    {

        string token = GetDeviceIdentifier() ?? "LegendKnight";
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(un), "\"un\"" },
                { new StringContent(pw), "\"pw\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}TryToken.php", content);
            string res = await response.Content.ReadAsStringAsync();
            var temps = JsonConvert.DeserializeObject<User>(res);
            if (temps.Token != null && temps.Token != "")
            {
                if (temps.Token != token)
                {
                    return "ErrorToken";
                }
            }
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

    }
    public static async Task<string> isuser(string un, string pw, string vc)
    {
        string vcToSend = vc ?? "1337";
        string token = GetDeviceIdentifier() ?? "LegendKnight";
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(un), "\"un\"" },
                { new StringContent(vcToSend), "\"vc\"" },
                { new StringContent(token), "\"token\"" },
                { new StringContent(pw), "\"pw\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckUser.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

    }
    public static async Task<string> UpdateUserData(int id)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "\"uid\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}KeepMeLoggedIn.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

    }
    public static async Task<string> CheckToken(int uid)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckToken.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

    }
    public static async Task<string> genvc(string id, int t)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(id), "\"id\"" },
                { new StringContent(t.ToString()), "\"type\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}GenerateVC.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

        /*try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Consts.APIUrl}GenerateVC.php?id={id}&&type={t}");
            var result = await response.Content.ReadAsStringAsync();
            return result + "test : "+id + t.ToString();
        }
        catch (Exception e)
        {
            //return "erorr404";
            return e.Message;
        }*/
    }
    public static async Task<string> checkvc(string id, string vc, int t)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(id), "\"id\"" },
                { new StringContent(vc), "\"vc\"" },
                { new StringContent(t.ToString()), "\"type\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckVC.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> CheckSignupCode(string Code)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(Code), "\"code\"" },
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckSignupCode.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }

    }
    public static async Task<string> CheckAccessFiles(string uid, string fid, string c = "LK")
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid), "\"uid\"" },
                { new StringContent(fid), "\"fid\"" },
            };
            if (c != "LK")
                content.Add(new StringContent(c), "\"code\"");

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckAccessFiles.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> CheckAccessTeachers(string uid, string tid, string c = "LK")
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid), "\"uid\"" },
                { new StringContent(tid), "\"tid\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
            };
            if (c != "LK")
                content.Add(new StringContent(c), "\"code\"");

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}CheckAccessTeacher.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> TeacherNewFolder(int uid, string fname, int parentfolderid)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(fname), "\"fname\"" },
                { new StringContent(parentfolderid.ToString()), "\"parentfid\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
            };

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}teachernewfolder.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> TeacherNewFileFromUrl(int uid, string fname, int parentfolderid,int typeid,string link)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(fname), "\"fname\"" },
                { new StringContent(parentfolderid.ToString()), "\"parentfid\"" },
                { new StringContent(typeid.ToString()), "\"typeid\"" },
                { new StringContent(link), "\"link\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
            };

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}teachernewfilefromurl.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> APIDelAcc(int id, string pw)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(id.ToString()), "\"uid\"" },
                { new StringContent(pw), "\"pw\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}DelAcc.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APIRecoverAcc(string email, string pw)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(email), "\"email\"" },
                { new StringContent(pw), "\"pw\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}RecoverAcc.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APIFinalDelAcc(int id, string pw)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(id.ToString()), "\"uid\"" },
                { new StringContent(pw), "\"pw\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}FinalDelAcc.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APISendFeedBack(int uid, int fbt, string fbinfo)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(fbt.ToString() == "0" ? "00" : fbt.ToString()), "\"fbt\"" },
                { new StringContent(fbinfo), "\"fbi\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Feedback.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> APIGetMyFeedbacks(int uid)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}GetmyFBs.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> UpdateUser(int uid, int t, string v, string ov = "lk")
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(uid.ToString()), "\"uid\"" },
            { new StringContent(t.ToString()), "\"t\"" },
            { new StringContent(v), "\"v\"" }
        };
        if (ov != "lk")
        {
            content.Add(new StringContent(ov), "\"ov\"");
        }
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync($"{Consts.APIUrl}UpdateUser.php", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    public static async Task<string> UpdateTeacherFile(int uid, int t,int fileid, string v, string ov = "lk")
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(uid.ToString()), "\"uid\"" },
            { new StringContent(fileid.ToString()), "\"fileid\"" },
            { new StringContent(t.ToString()), "\"t\"" },
            { new StringContent(v), "\"v\"" }
        };
        if (ov != "lk")
        {
            content.Add(new StringContent(ov), "\"ov\"");
        }
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync($"{Consts.APIUrl}UpdateTeacherFile.php", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    public static async Task<string> TeacherInsertInto_AccessFilesCodes(int uid, int type, int tofid
        ,string pattern,int lenght , int NOUATUTC,string ValidTo, int CodesCount)
    {
        //type : 1 for Teacher code , 0 File Code
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(uid.ToString()), "\"uid\"" },
                { new StringContent(GetDeviceIdentifier()), "\"token\"" },
                { new StringContent(type.ToString()), "\"type\"" },
                { new StringContent(tofid.ToString()), "\"tofid\"" },
                { new StringContent(pattern), "\"pattern\"" },
                { new StringContent(lenght.ToString()), "\"length\"" },
                { new StringContent(NOUATUTC.ToString()), "\"NOUATUTC\"" },
                { new StringContent(ValidTo), "\"ValidTo\"" },
                { new StringContent(CodesCount.ToString()), "\"CodesCount\"" },
            };

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}teacherCreateCodes.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return "erorr404";
            //return e.Message;
        }
    }
    public static async Task<string> UploadAttachment(Stream streamtosend , string fn , int type)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(streamtosend), "\"file\"", $"\"{System.Net.WebUtility.UrlEncode(fn)}\"" },
            { new StringContent(type.ToString()), "\"type\"" }
        };
        var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(300)
        };
        var response = await httpClient.PostAsync(Consts.APIUrl + "uploading.php", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    public static async Task<string> APIGetTeachers()
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}GetTeachers.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static async Task<string> Admin_APIAllFeedbacks(int uid)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent("LegendK"), "\"token\"" },
                { new StringContent(uid.ToString()), "\"uid\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}GetFBS.php", content);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static string GetSubjectsString(ObservableCollection<Subject> Subjects)
    {
        var nonEmptySubjects = Subjects.Where(s => !string.IsNullOrWhiteSpace(s.Name));
        return "[ " + string.Join(", ", nonEmptySubjects.Select(s => $"\"{s.Name}\"")) + " ]";
    }
    public static ObservableCollection<Subject> GetSubjectsList(List<string> Subjects)
    {
        ObservableCollection<Subject> rv = new ObservableCollection<Subject>();
        foreach (string s in Subjects)
            rv.Add(new Subject { Name = s });
        return rv;
    }
    public static async Task<int> VideoCount(int UserId, int VideoID)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(UserId.ToString()), "\"uid\"" },
                { new StringContent(VideoID.ToString()), "\"vid\"" }
            };
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{Consts.APIUrl}Getvideocount.php", content);
            string res = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(res);
            var root = doc.RootElement;

            if (root.TryGetProperty("VideoCounter", out JsonElement counterElement))
            {
                return counterElement.GetInt32();
            }
            else
            {
                // لو مفيش VideoCounter في الرد
                return -1;
            }
        }
        catch
        {
            // في حالة أي Error
            return -1;
        }



    }

}
public static class TapEventHandler
{
    private static bool _isNavigating = false;

    public static void Reset_isNavigating()
    {
        _isNavigating = false;
    }
    public static async Task<bool> HandleTapEvent(bool WithConnCheck, Func<Task> action, INavigation navigation)
    {
        if (_isNavigating)
            return false;

        _isNavigating = true;
        try
        {

            bool isConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (WithConnCheck && !isConnected)
            {
                await MsgWithIcon.ShowNoConn(navigation);
                return false; // Indicate that the action was not executed due to no connection
            }

            await action();
            return true; // Indicate that the action was successfully executed
        }
        finally
        {
            _isNavigating = false;
        }
    }

}
public static class Validate
{
    public enum ValidateType
    {
        None,
        Email,
        Password,
        Name,
        Phone,
        PasswordConfirmation
    }

    public static bool Email(string s)
    {
        return string.IsNullOrWhiteSpace(s) || Regex.IsMatch(s, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    }
    public static bool Password(string s)
    {
        return !string.IsNullOrWhiteSpace(s) && s.Length >= 6;
    }
    public static bool Name(string s)
    {
        return (string.IsNullOrWhiteSpace(s) || s.Length >= 3);
    }
    public static bool Phone(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return true;
        return (s.Length == 11 && s.StartsWith("01")) ||
               (s.Length == 13 && s.StartsWith("+201"));
    }
    public static bool PasswordConfirm(string s, string Mainpw)
    {
        if (string.IsNullOrWhiteSpace(s))
            return true;
        //return (s == MainPassword);
        return string.Equals(s, Mainpw, StringComparison.Ordinal);
    }

    public static async Task<bool> UserSession()
    {
        string rv = await GlobalFunc.CheckToken(AL_HomePage.CU.UserID);
        if (rv != "succ")
        {
            GlobalFunc.ToastShow("لقد انتهت صلاحية جلسة تسجيل الدخول الخاصة بك، يرجى إعادة تسجيل الدخول");
            AL_HomePage.CU = null;
            return false;
        }
        return true;
    }
    public static async Task<bool> CheckForUpdate()
    {
        var currentVersion = VersionTracking.CurrentVersion;
        var latestVersion = await GlobalFunc.APIGetAppVersion();
        if (latestVersion != "Error 404")
            return Version.Parse(currentVersion) < Version.Parse(latestVersion);
        else
        {
            GlobalFunc.ToastShow("Bad Request!");
            return true;
        }
    }
    public static async Task<bool> AppVersion()
    {
        var currentVersion = VersionTracking.CurrentVersion;
        var latestValidVersion = await GlobalFunc.APIGetAppVersion(2);
        if (latestValidVersion != "Error 404")
            return Version.Parse(currentVersion) >= Version.Parse(latestValidVersion);
        else
        {
            GlobalFunc.ToastShow("Bad Request!");
            return false;
        }
    }
}
public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}
public class FileFontResolver : IFontResolver
{
    public string DefaultFontName => "Poppins-Medium";

    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resource = "Ang7.Resources.Fonts.Poppins-Medium.ttf";
        using var stream = assembly.GetManifestResourceStream(resource);
        if (stream == null)
            throw new InvalidOperationException($"Resource not found: {resource}");

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // يمكنك إضافة منطق اختيار الخط حسب التسمية هنا
        return new FontResolverInfo("Poppins-Medium");
    }
}