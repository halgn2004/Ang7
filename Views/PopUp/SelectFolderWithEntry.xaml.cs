using Ang7.Models;
using System.Collections.ObjectModel;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectFolderWithEntry : PopupPage
{
    private ObservableCollection<TeacherFile> allFolders; // All folders
    private ObservableCollection<TeacherFile> displayedFolders; // Folders to display
    private int currentParentFolderID; // Track the current folder
    private readonly Action<int, string> setResultAction;

    public SelectFolderWithEntry(string Title,ObservableCollection<TeacherFile> folders, Action<int, string> sra, string placeholder, string value)
    {
        InitializeComponent();
        HeadLabel.Text = Title;
        allFolders = folders;
        entry.Placeholder = placeholder;
        entry.Text = value;

        currentParentFolderID = 0; // Start at the root level
        LoadFoldersForCurrentFolder();
        setResultAction = sra;
    }
    /*protected override bool OnBackButtonPressed()
{
    PopupNavigation.Instance.PopAsync();
    return true;
}*/

    // Load folders for the current parent folder
    private void LoadFoldersForCurrentFolder()
    {
        displayedFolders = new ObservableCollection<TeacherFile>(
            allFolders.Where(f => f.ParentFolderID == currentParentFolderID && f.IsFolder)
        );

        FolderList.ItemsSource = displayedFolders;
        FolderList.HeightRequest = displayedFolders.Count * 50 + 5;
        BackButton.IsVisible = currentParentFolderID != 0; // Show back button if not at root
    }

    private void OnClose_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
    }

    private void OnBack_Clicked(object sender, EventArgs e)
    {
        var parentFolder = allFolders.FirstOrDefault(f => f.ID == currentParentFolderID);
        if (parentFolder != null)
        {
            currentParentFolderID = parentFolder.ParentFolderID; // Move up to parent
            LoadFoldersForCurrentFolder();
        }
    }

    // Handle folder selection
    private void FolderList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectedFolder = e.Item as TeacherFile;
        if (selectedFolder.IsFolder)
        {
            currentParentFolderID = selectedFolder.ID; // Navigate into the folder
            LoadFoldersForCurrentFolder();
        }
    }
    // Confirm selection and close the popup
    private void OnConfirm_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(currentParentFolderID,entry.Text);
        // Return the selected folder ID (optional, can be done in another way)
    }

    // Utility function for displaying the folder popup
    public static async Task<(int, string)> Select(string title, ObservableCollection<TeacherFile> folders, INavigation nav, string placeholder, string value)
    {
        TaskCompletionSource<(int, string)> cs = new TaskCompletionSource<(int, string)>();
        void callback(int didconfirm,string n)
        {
            cs.TrySetResult((didconfirm,n));
        }
        var pop = new SelectFolderWithEntry(title,folders, callback, placeholder, value);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}