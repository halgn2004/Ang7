
using Ang7.ViewModels;
using Ang7.Views;
using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using PanCardView;
using Plugin.Maui.ScreenSecurity;
using RGPopup.Maui.Extensions;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MauiPageFullScreen;
using Ang7.Handlers;
using Ang7.Helpers;
using Plugin.Maui.AppRating;
using Microsoft.Maui.Handlers;
using Maui.PDFView;


#if ANDROID
using Ang7.Platforms.Android;
using Ang7.Platforms.Android.Helpers;
#elif IOS
using Ang7.Platforms.iOS;
using Ang7.Platforms.iOS.Helpers;
using UIKit;
using AVKit;
using CommunityToolkit.Maui.Core.Handlers;
#endif
namespace Ang7;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .UseSkiaSharp()
            .UseFFImageLoading()
            .UseCardsView()
            .UseMauiRGPopup(config =>
            {
                config.BackPressHandler = null;
                config.FixKeyboardOverlap = true;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("CenturyGothicRegular.ttf", "CenturyGothic-Regular");
                fonts.AddFont("ERASLGHT.ttf", "ERASLGHT");
                fonts.AddFont("fa-regular-400.ttf", "fa-regular");
                fonts.AddFont("fa-solid-900.ttf", "fa-solid");
                fonts.AddFont("FjallaOne-Regular.ttf", "FjallaOne-Regular");
                fonts.AddFont("Poppins-Medium.ttf", "Poppins-Medium");
                fonts.AddFont("Poppins-Regular.ttf", "Poppins-Regular");
                fonts.AddFont("Poppins-SemiBold.ttf", "Poppins-SemiBold");
                fonts.AddFont("Roboto-Light.ttf", "Roboto-Light");
                fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                /*Main Fonts*/
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseScreenSecurity()
            .UseFullScreen()
            ;
        builder.UseMauiPdfView();
        ConfigureCustomHandlers(builder);
        // Register platform-specific services
#if ANDROID
        builder.Services.AddSingleton<IRotateInterface, RotateService>();
#elif IOS
        builder.Services.AddSingleton<IRotateInterface, RotateService>();
#endif
        builder.Services.AddSingleton<SS>();
        builder.Services.AddSingleton<Intro_1>();
        builder.Services.AddSingleton<Intro_2>();

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<CarouselHomePageViewModel>();

        builder.Services.AddTransient<SigninPage>();
        builder.Services.AddTransient<SigninPageViewModel>();
        builder.Services.AddTransient<SignupPage>();
        builder.Services.AddTransient<SignupPageViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddTransient<AL_HomePage>();
        builder.Services.AddTransient<AL_HomePageViewModel>();
        builder.Services.AddTransient<AL_Settings>();
        builder.Services.AddTransient<AL_Profile>();
        builder.Services.AddTransient<AL_ProfileViewModel>();
        builder.Services.AddTransient<AL_TeacherFilesManger>();
        builder.Services.AddTransient<AL_TeacherFilesMangerViewModel>();

        builder.Services.AddTransient<AL_TeacherView>();
        builder.Services.AddTransient<AL_TeacherViewModel>();

        builder.Services.AddTransient<FeedbackMsg>();
        builder.Services.AddTransient<FeedbackViewModel>();

        builder.Services.AddTransient<AdminShowFeedbacks>();

        builder.Services.AddTransient<PlayVideoPage>();
        builder.Services.AddTransient<PDFViewPage>();
        builder.Services.AddTransient<ImageViewPage>();

        // Register plugin
        builder.Services.AddSingleton(ScreenSecurity.Default);
        builder.Services.AddSingleton(AppRating.Default);

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var lkapp = builder.Build();

        // Set the service provider for the static ServiceLocator
        ServiceLocator.SetServiceProvider(lkapp.Services);

        return lkapp;
    }
    private static void ConfigureCustomHandlers(MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(BorderlessEntry), typeof(EntryHandler));
            handlers.AddHandler(typeof(EditorWithoutUnderline), typeof(EditorHandler));
            //handlers.AddHandler(typeof(Video), typeof(VideoHandler));
#if IOS
            handlers.AddHandler(typeof(Shell), typeof(CustomShellRenderer));  
#endif
        });
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
            }
        });
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(EditorWithoutUnderline), (handler, view) =>
        {
            if (view is EditorWithoutUnderline)
            {
#if ANDROID
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                //handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.TextAlignment = UITextAlignment.Right;
#elif WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
            }
        });
    }
}
