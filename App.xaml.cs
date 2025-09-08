using PdfSharpCore.Fonts;
namespace Ang7;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        GlobalFontSettings.FontResolver = new FileFontResolver();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

}