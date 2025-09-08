using Ang7.Handlers;

namespace Ang7.CustomControls;

public partial class CustomCodeEntry : ContentView
{
    public static readonly BindableProperty CodeProperty = BindableProperty.Create(
        nameof(Code),
        typeof(string),
        typeof(CustomCodeEntry),
        string.Empty,
        BindingMode.TwoWay);

    public string Code
    {
        get => (string)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    public CustomCodeEntry()
    {
        InitializeComponent();
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        BorderlessEntry currentEntry = sender as BorderlessEntry;

        /*if(e.NewTextValue.Length > 1){
            Entry1.Text = e.NewTextValue[0].ToString();
            Entry2.Text = e.NewTextValue[1].ToString();
            Entry3.Text = e.NewTextValue[2].ToString();
            Entry4.Text = e.NewTextValue[3].ToString();
            Entry5.Text = e.NewTextValue[4].ToString();
            Entry6.Text = e.NewTextValue[5].ToString();
            Code = e.NewTextValue;
            currentEntry.Unfocus();
            return;
        }*/

        // Automatically move focus to the next entry
        if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length == 1)
        {
            if (currentEntry == Entry1) Entry2.Focus();
            else if (currentEntry == Entry2) Entry3.Focus();
            else if (currentEntry == Entry3) Entry4.Focus();
            else if (currentEntry == Entry4) Entry5.Focus();
            else if (currentEntry == Entry5) Entry6.Focus();
            else if (currentEntry == Entry6) Entry6.Unfocus();
        }
        else if (string.IsNullOrEmpty(e.NewTextValue) && currentEntry != Entry1)
        {
            // Move focus to the previous entry if the current entry is cleared
            if (currentEntry == Entry2) Entry1.Focus();
            else if (currentEntry == Entry3) Entry2.Focus();
            else if (currentEntry == Entry4) Entry3.Focus();
            else if (currentEntry == Entry5) Entry4.Focus();
            else if (currentEntry == Entry6) Entry5.Focus();
        }

        // Update the Code property to reflect the combined entry values
        Code = $"{Entry1.Text}{Entry2.Text}{Entry3.Text}{Entry4.Text}{Entry5.Text}{Entry6.Text}";
    }
}