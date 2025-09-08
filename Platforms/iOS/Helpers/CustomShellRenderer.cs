using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Ang7.Platforms.iOS.Helpers;

public class CustomShellRenderer : ShellRenderer
{
    public CustomShellRenderer()
    {

    }
    protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
    {
        return new CustomSectionRenderer(this);
    }
}

public class CustomSectionRenderer : ShellSectionRenderer
{
    public CustomSectionRenderer(IShellContext context) : base(context)
    {

    }
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        InteractivePopGestureRecognizer.Enabled = false;
    }
}