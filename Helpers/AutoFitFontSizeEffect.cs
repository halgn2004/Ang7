namespace Ang7.Helpers
{
    public class AutoFitFontSizeEffectParameters
    {
        public static BindableProperty MaxFontSizeProperty = BindableProperty.CreateAttached(
            "MaxFontSize",
            typeof(double),
            typeof(AutoFitFontSizeEffectParameters),
            20.0, // Default max size
            BindingMode.Default
        );

        public static BindableProperty MinFontSizeProperty = BindableProperty.CreateAttached(
            "MinFontSize",
            typeof(double),
            typeof(AutoFitFontSizeEffectParameters),
            10.0, // Default min size
            BindingMode.Default
        );

        public static double GetMaxFontSize(BindableObject bindable)
        {
            return (double)bindable.GetValue(MaxFontSizeProperty);
        }

        public static double GetMinFontSize(BindableObject bindable)
        {
            return (double)bindable.GetValue(MinFontSizeProperty);
        }

        public static void SetMaxFontSize(BindableObject bindable, double value)
        {
            bindable.SetValue(MaxFontSizeProperty, value);
        }

        public static void SetMinFontSize(BindableObject bindable, double value)
        {
            bindable.SetValue(MinFontSizeProperty, value);
        }
    }

    public class AutoFitFontSizeEffect : RoutingEffect
    {
        public AutoFitFontSizeEffect() : base($"Ang7.Helpers.{nameof(AutoFitFontSizeEffect)}")
        {
        }
    }
}
