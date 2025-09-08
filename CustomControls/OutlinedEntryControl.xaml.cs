using static Ang7.Validate;

namespace Ang7.CustomControls;

public partial class OutlinedEntryControl : Grid
{
	public OutlinedEntryControl()
	{
		InitializeComponent();
        AnimatePlaceholder(!string.IsNullOrWhiteSpace(Text));
    }

    /*public static readonly BindableProperty MinLengthProperty = BindableProperty.Create(
    propertyName: nameof(MinLength),
    returnType: typeof(int),
    declaringType: typeof(OutlinedEntryControl),
    defaultValue: 0,
    defaultBindingMode: BindingMode.OneWay);

    public int MinLength
    {
        get => (int)GetValue(MinLengthProperty);
        set => SetValue(MinLengthProperty, value);
    }*/

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        propertyName: nameof(MaxLength),
        returnType: typeof(int),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: int.MaxValue,
        defaultBindingMode: BindingMode.OneWay);

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: OnTextChanged);
    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (OutlinedEntryControl)bindable;
        control.ValidateInput();
        // Animate placeholder based on whether text is empty or not
        control.AnimatePlaceholder(!string.IsNullOrWhiteSpace(control.Text));
    }
    private void ValidateInput()
    {
        bool isValid = ValidationType switch
        {
            ValidateType.Email => Email(Text),
            ValidateType.Password => Password(Text),
            ValidateType.Name => Name(Text),
            ValidateType.Phone => Phone(Text),
            ValidateType.PasswordConfirmation => PasswordConfirm(Text, MainPassword),
            _ => true
        };

        IsValidationVisible = !isValid;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
      propertyName: nameof(Placeholder),
      returnType: typeof(string),
      declaringType: typeof(OutlinedEntryControl),
      defaultValue: null,
      defaultBindingMode: BindingMode.OneWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set { SetValue(PlaceholderProperty, value); }
    }


    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(OutlinedEntryControl), Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }


    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
    propertyName: nameof(IsPassword),
    returnType: typeof(bool),
    declaringType: typeof(OutlinedEntryControl),
    defaultValue: false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set
        {
            SetValue(IsPasswordProperty, value);
        }
    }
    public static readonly BindableProperty IsShowPasswordButtonProperty = BindableProperty.Create(
    propertyName: nameof(IsShowPasswordButton),
    returnType: typeof(bool),
    declaringType: typeof(OutlinedEntryControl),
    defaultValue: false);

    public bool IsShowPasswordButton
    {
        get => (bool)GetValue(IsShowPasswordButtonProperty);
        set
        {
            SetValue(IsShowPasswordButtonProperty, value);
        }
    }
    public static readonly BindableProperty ValidationMessageProperty = BindableProperty.Create(
    propertyName: nameof(ValidationMessage),
    returnType: typeof(string),
    declaringType: typeof(OutlinedEntryControl),
    defaultValue: string.Empty,
    defaultBindingMode: BindingMode.OneWay);

    public string ValidationMessage
    {
        get => (string)GetValue(ValidationMessageProperty);
        set => SetValue(ValidationMessageProperty, value);
    }

    public static readonly BindableProperty IsValidationVisibleProperty = BindableProperty.Create(
        propertyName: nameof(IsValidationVisible),
        returnType: typeof(bool),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay);

    public bool IsValidationVisible
    {
        get => (bool)GetValue(IsValidationVisibleProperty);
        set => SetValue(IsValidationVisibleProperty, value);
    }

    public static readonly BindableProperty MainPasswordProperty = BindableProperty.Create(
        propertyName: nameof(MainPassword),
        returnType: typeof(string),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay);

    public string MainPassword
    {
        get => (string)GetValue(MainPasswordProperty);
        set => SetValue(MainPasswordProperty, value);
    }

    public static readonly BindableProperty ValidationTypeProperty = BindableProperty.Create(
        propertyName: nameof(ValidationType),
        returnType: typeof(ValidateType),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: ValidateType.None);

    public ValidateType ValidationType
    {
        get => (ValidateType)GetValue(ValidationTypeProperty);
        set => SetValue(ValidationTypeProperty, value);
    }


    private void txtEntry_Focused(object sender, FocusEventArgs e)
    {
        AnimatePlaceholder(true);
    }

    private void txtEntry_Unfocused(object sender, FocusEventArgs e)
    {
        AnimatePlaceholder(!string.IsNullOrWhiteSpace(Text));
    }
    private void AnimatePlaceholder(bool isFocused)
    {
        if (DeviceInfo.Idiom==DeviceIdiom.Tablet){
            lblPlaceholder.FontSize = isFocused ? 22 : 30;
            lblPlaceholder.TranslateTo(0, isFocused ? -25 : 0, 80, easing: Easing.Linear);//-20,80
        }else{
            lblPlaceholder.FontSize = isFocused ? 11 : 15;
            lblPlaceholder.TranslateTo(0, isFocused ? -23 : 0, 80, easing: Easing.Linear);//-20,80
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        txtEntry.Focus();
    }
    private void TapGestureRecognizer_ShowPassword(object sender, EventArgs e)
    {
        IsPassword = !IsPassword;
    }

}