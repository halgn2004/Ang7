using Ang7.Helpers;

namespace Ang7.CustomControls;

public partial class PasswordPatternPicker : ContentView
{
    public PasswordPatternPicker()
    {
        InitializeComponent();

        // Initialize default states and sync with settings
        SelectedPattern = Settings.PWPatters_Pattern;
        UpdateCheckboxes(SelectedPattern);

        CodeLength = Settings.PWPatters_CodeLenght;  // Default code length

        // Populate CodeLengthPicker with values 6 to 15
        for (int i = 6; i <= 15; i++)
        {
            CodeLengthPicker.Items.Add(i.ToString());
        }
        CodeLengthPicker.SelectedIndex = CodeLength - 6;
    }

    public static readonly BindableProperty SelectedPatternProperty =
        BindableProperty.Create(nameof(SelectedPattern), typeof(string), typeof(PasswordPatternPicker), "1000",
            BindingMode.TwoWay, propertyChanged: OnSelectedPatternChanged);

    public static readonly BindableProperty CodeLengthProperty =
        BindableProperty.Create(nameof(CodeLength), typeof(int), typeof(PasswordPatternPicker), 8,
            BindingMode.TwoWay, propertyChanged: OnCodeLengthChanged);

    public string SelectedPattern
    {
        get => (string)GetValue(SelectedPatternProperty);
        set => SetValue(SelectedPatternProperty, value);
    }

    public int CodeLength
    {
        get => (int)GetValue(CodeLengthProperty);
        set => SetValue(CodeLengthProperty, value);
    }

    private static void OnSelectedPatternChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PasswordPatternPicker)bindable;
        control.UpdateCheckboxes(newValue as string);
    }

    private static void OnCodeLengthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PasswordPatternPicker)bindable;
        if (int.TryParse(newValue.ToString(), out int newLength))
        {
            control.CodeLengthPicker.SelectedIndex = newLength - 6;
        }
    }

    private void UpdateCheckboxes(string pattern)
    {
        if (pattern?.Length == 4)
        {
            NumbersCheckBox.IsChecked = pattern[0] == '1';
            UppercaseCheckBox.IsChecked = pattern[1] == '1';
            LowercaseCheckBox.IsChecked = pattern[2] == '1';
            SymbolsCheckBox.IsChecked = pattern[3] == '1';
        }
    }

    private void OnPatternChanged(object sender, CheckedChangedEventArgs e)
    {
        // Prevent all checkboxes from being unchecked
        if (!NumbersCheckBox.IsChecked && !UppercaseCheckBox.IsChecked &&
            !LowercaseCheckBox.IsChecked && !SymbolsCheckBox.IsChecked)
        {
            NumbersCheckBox.IsChecked = true;
            SelectedPattern = "1000"; // Default to numbers only
            return;
        }

        SelectedPattern = (NumbersCheckBox.IsChecked ? "1" : "0") +
                          (UppercaseCheckBox.IsChecked ? "1" : "0") +
                          (LowercaseCheckBox.IsChecked ? "1" : "0") +
                          (SymbolsCheckBox.IsChecked ? "1" : "0");
    }
}
