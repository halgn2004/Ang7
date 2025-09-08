using Ang7.PDFCode;
using Microsoft.Maui.Controls;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using RGPopup.Maui.Services;
using SkiaSharp;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;






namespace Ang7.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]

public partial class PDFViewPage : ContentPage
{
    private int currentPage = 1;
    private int totalPages = 5;

    public PDFViewPage(string t, string l)
    {
        InitializeComponent();
        var vm = new MainPageViewModel();

        // مسار الملف بعد التعديل مباشرة (مش هنستخدم input.pdf خالص)
        string outputPath = Path.Combine(FileSystem.CacheDirectory, "output.pdf");

        // تحميل الملف ثم عمل العلامة المائية فوراً
        using (var client = new WebClient())
        {
            string tempPath = Path.Combine(FileSystem.CacheDirectory, "temp_download.pdf");
            client.DownloadFile(l, tempPath);

            // إضافة العلامة المائية وحفظه في output.pdf
            AddWatermark(tempPath, outputPath, $"User:{AL_HomePage.CU.UserID} \n \t Name: {AL_HomePage.CU.Name}");

            // ممكن نحذف الملف الأصلي المؤقت بعد التعديل
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }

        Title.Text = t;
        vm.PdfSource = outputPath;
        BindingContext = vm;
    }

    private void AddWatermark(string inputPath, string outputPath, string watermarkText)
    {
        using (PdfDocument document = PdfReader.Open(inputPath, PdfDocumentOpenMode.Modify))
        {
            // تحميل الصورة
            var assembly = Assembly.GetExecutingAssembly();

            using Stream stream = assembly.GetManifestResourceStream("Ang7.Resources.Images.wt2.png");
            if (stream == null)
                throw new FileNotFoundException("Image not found!");

            // 3- نحولها لصورة XImage
            XImage img = XImage.FromStream(() => stream);

            // دلوقتي تقدر تبعته لـ PDFsharp أو أي مكتبة عايزة مسار مباشر


            // مقاس الصورة
            double imgWidth = 400;
            double imgHeight = 500;


            foreach (var page in document.Pages)
            {
                double xx = (page.Width - imgWidth) / 2;
                double yy = (page.Height - imgHeight) / 2;

                XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                var font = new XFont("Poppins-Medium", 20, XFontStyle.Bold); // حجم أصغر عشان التكرار
                XBrush brush = new XSolidBrush(XColor.FromArgb(50, 2, 1, 1)); // شفافية أعلى

                double stepX = 200; // المسافة الأفقية بين كل وسم والتاني
                double stepY = 150; // المسافة الرأسية بين كل وسم والتاني
                gfx.DrawImage(img, xx, yy, imgWidth, imgHeight);

                for (double y = 0; y < page.Height; y += stepY)
                {

                    for (double x = 0; x < page.Width; x += stepX)
                    {
                        gfx.TranslateTransform(x, y);
                        gfx.RotateTransform(-45);
                        gfx.DrawString(watermarkText, font, brush, new XPoint(0, 0), XStringFormats.Center);
                        gfx.RotateTransform(45);
                        gfx.TranslateTransform(-x, -y);
                    }
                }
            }
            document.Save(outputPath);
            if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Any())
                PopupNavigation.Instance.PopAsync();
        }
    }

    /*private void OnStartSS(object sender, EventArgs e)
    {
        //AppShell.GoToPage(nameof(HomePage));
        Console.WriteLine("byscrren ");
    }*/
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override bool OnBackButtonPressed()
    {
        Navigation.PopAsync();
        return true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        //await AppShell.GoToPage("..");
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        //ScreenSecurity.Default.ScreenCaptured -= OnStartSS;
    }

}
