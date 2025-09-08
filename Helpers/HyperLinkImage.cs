using FFImageLoading.Maui;

namespace Ang7.Helpers;

public class HyperLinkImage : CachedImage
{
    public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperLinkImage), null);

    public string Url
    {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }

    public HyperLinkImage()
    {
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => await Launcher.OpenAsync(Url))
        });
    }
}
