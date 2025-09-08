using RGPopup.Maui.Pages; // Ensure you have the correct using directive
using RGPopup.Maui.Services;
using System.Net;
namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PDFViewer : PopupPage
{
    public PDFViewer(string N, string L)
    {
        InitializeComponent();
        Title.Text = N;
        WatermarkView.BadgeText = AL_HomePage.CU.UserID.ToString();
#if ANDROID
        // Optional: Enable WebView settings if necessary
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("pdfviewer", (handler, view) =>
                {
                    handler.PlatformView.Settings.AllowFileAccess = true;
                    //handler.PlatformView.Settings.AllowUniversalAccessFromFileURLs = true;
                    //handler.PlatformView.Settings.JavaScriptEnabled = true;
                });

                //Webview.Source = "https://docs.google.com/viewer?url=" + WebUtility.UrlEncode(L); ;
                Webview.Source = "https://drive.google.com/viewerng/viewer?embedded=true&url=" + WebUtility.UrlEncode(L); ;
                
#else
        Webview.Source = L;
        #endif

        /* Use Microsoft.Maui.Devices.DeviceInfo to determine platform
        if (Microsoft.Maui.Devices.DeviceInfo.Platform == Microsoft.Maui.Devices.DevicePlatform.Android)
        {
            Webview.Uri = L;
            // Set zoom controls
            Webview.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().EnableZoomControls(true);
            Webview.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().DisplayZoomControls(false);
        }
        else
        {
            Webview.Source = L;
        }*/

    }


    protected override bool OnBackButtonPressed()
    {
        //PopupNavigation.Instance.PopAsync();
        return false;
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WatermarkView.StopAnimations();
        //ServiceLocator.GetService<IRotateInterface>().DisableRotation();
        //Controls.RestoreScreen();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //Controls.FullScreen();
        //ServiceLocator.GetService<IRotateInterface>().EnableRotation();
    }

}
