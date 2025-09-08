using OnCheckedChanged = System.EventHandler<Microsoft.Maui.Controls.CheckedChangedEventArgs>;

namespace Ang7.CustomControls;

public partial class CheckBoxLabeled : HorizontalStackLayout, ICheckBox, ILabel
{
    #region CheckBox
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.CreateAttached(
        propertyName: nameof(IsChecked),
        returnType: typeof(bool),
        declaringType: typeof(CheckBoxLabeled),
        defaultValue: CheckBox.IsCheckedProperty.DefaultValue,
        defaultBindingMode: CheckBox.IsCheckedProperty.DefaultBindingMode);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set { SetValue(IsCheckedProperty, value); }
    }

    //public static readonly BindableProperty ColorProperty = BindableProperty.Create(
    //    propertyName: nameof(Color),
    //    returnType: typeof(Color),
    //    declaringType: typeof(CheckBoxLabeled),
    //    defaultValue: CheckBox.ColorProperty.DefaultValue,
    //    defaultBindingMode: CheckBox.ColorProperty.DefaultBindingMode);
    //
    //public Color Color
    //{
    //    get => (Color)GetValue(ColorProperty);
    //    set { SetValue(ColorProperty, value ?? _initialColor); }
    //}

    protected readonly Color _initialColor;

    public Color Color
    {
        get => CheckBox.Color;
        set { CheckBox.Color = value ?? _initialColor; }
    }

    public Paint Foreground => CheckBox.Foreground;

    public event OnCheckedChanged CheckedChanged;
    #endregion

    #region Label
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(CheckBoxLabeled),
        defaultValue: "",
        defaultBindingMode: BindingMode.OneWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set { SetValue(TextProperty, value); }
    }

    protected readonly Color _initialTextColor;

    public Color TextColor
    {
        get => Label.TextColor;
        set { Label.TextColor = value ?? _initialTextColor; }
    }

    Microsoft.Maui.Font ITextStyle.Font => ((ITextStyle)Label).Font;

    public double CharacterSpacing { get => Label.CharacterSpacing; set => Label.CharacterSpacing = value; }

    public TextAlignment HorizontalTextAlignment { get => Label.HorizontalTextAlignment; set => Label.HorizontalTextAlignment = value; }

    public TextAlignment VerticalTextAlignment { get => Label.VerticalTextAlignment; set => Label.VerticalTextAlignment = value; }

    public bool FontAutoScalingEnabled { get => Label.FontAutoScalingEnabled; set => Label.FontAutoScalingEnabled = value; }

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double FontSize { get => Label.FontSize; set => Label.FontSize = value; }

    public string FontFamily { get => Label.FontFamily; set => Label.FontFamily = value; }

    public TextDecorations TextDecorations { get => Label.TextDecorations; set => Label.TextDecorations = value; }

    public FontAttributes FontAttributes { get => Label.FontAttributes; set => Label.FontAttributes = value; }

    public double LineHeight { get => Label.LineHeight; set => Label.LineHeight = value; }
    #endregion

    public bool CheckChangeOnLableTap { get; set; } = true;

    public CheckBoxLabeled()
    {
        InitializeComponent();

        _initialColor = CheckBox.Color;
        _initialTextColor = Label.TextColor;
    }

    protected void OnLabelClicked(object sender, EventArgs e)
    {
        if (CheckChangeOnLableTap) CheckBox.IsChecked = !CheckBox.IsChecked;
    }

    protected void OnCheckChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckedChanged?.Invoke(this, e);
    }
}