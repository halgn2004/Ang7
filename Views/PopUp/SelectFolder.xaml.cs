using Ang7.Models;
using System.Collections.ObjectModel;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Ang7.Views.PopUp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectFolder : PopupPage
{
    private ObservableCollection<TeacherFile> allFolders; // All folders
    private ObservableCollection<TeacherFile> displayedFolders; // Folders to display
    private int currentParentFolderID; // Track the current folder
    //private int currentFolderID = -1; // Track the current folder
    private readonly Action<string> setResultAction;

    string mTitle;

    public SelectFolder(string Title,ObservableCollection<TeacherFile> folders, Action<string> sra)
    {
        InitializeComponent();
        HeadLabel.Text = Title;
        mTitle = Title;
        allFolders = folders;

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
            if (parentFolder.ParentFolderID < 1)
                HeadLabel.Text = mTitle;
            else
                HeadLabel.Text = mTitle + " ( " + parentFolder.Name + " )";
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
            HeadLabel.Text = mTitle + " ( " + selectedFolder.Name + " )";
        }
    }
    // Confirm selection and close the popup
    private void OnConfirm_Clicked(object sender, EventArgs e)
    {
        PopupNavigation.Instance.PopAsync();
        setResultAction?.Invoke(currentParentFolderID.ToString());
        // Return the selected folder ID (optional, can be done in another way)
    }

    // Utility function for displaying the folder popup
    public static async Task<string> Select(string title, ObservableCollection<TeacherFile> folders, INavigation nav)
    {
        TaskCompletionSource<string> cs = new TaskCompletionSource<string>();
        void callback(string didconfirm)
        {
            cs.TrySetResult(didconfirm);
        }
        var pop = new SelectFolder(title,folders, callback);
        await nav.PushPopupAsync(pop);

        return await cs.Task;
    }
}