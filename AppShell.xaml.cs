using Ang7.Views;

namespace Ang7;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Register additional routes (not accessible via Shell navigation UI)
        Routing.RegisterRoute(nameof(Intro_2), typeof(Intro_2));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

        Routing.RegisterRoute(nameof(AL_TeacherView), typeof(AL_TeacherView));
        Routing.RegisterRoute(nameof(AL_TeacherFilesManger), typeof(AL_TeacherFilesManger));
        Routing.RegisterRoute(nameof(AL_Profile), typeof(AL_Profile));
        Routing.RegisterRoute(nameof(AL_Settings), typeof(AL_Settings));
        Routing.RegisterRoute(nameof(FeedbackMsg), typeof(FeedbackMsg));
        Routing.RegisterRoute(nameof(AdminShowFeedbacks), typeof(AdminShowFeedbacks));
        Routing.RegisterRoute(nameof(PlayVideoPage), typeof(PlayVideoPage));
        Routing.RegisterRoute(nameof(ImageViewPage), typeof(ImageViewPage));
        Routing.RegisterRoute(nameof(PDFViewPage), typeof(PDFViewPage));
    }
    public static async Task GoToPage(string Page, ShellNavigationQueryParameters myparams = null)
    {
        try
        {
            string target = Page switch
            {
                nameof(SS)
                or nameof(Intro_1)
                or nameof(HomePage) or nameof(AL_HomePage)
                or nameof(SigninPage) or nameof(SignupPage) => $"//{Page}",
                _ => Page
            };

            if (myparams != null && myparams.Any())
            {
                var query = string.Join("&", myparams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value.ToString())}"));
                target += $"?{query}";
            }
            Console.WriteLine(target);
            await Shell.Current.GoToAsync(target);
        }
        catch (Exception ex)
        {
            GlobalFunc.ToastShow($"Navigation error: {ex.Message}");
            Console.WriteLine($"Navigation error: {ex.Message}");
        }

    }

}
