using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Ang7.ViewModels;

public class PopUpSelectAvatarViewModel : BaseViewModel
{
    public ObservableCollection<object> Items { get; }
    private int _currentIndex;
    public int CurrentIndex
    {
        get { return _currentIndex; }
        set { if (SetProperty(ref _currentIndex, value)) UpdateUI(value); }
    }
    private int _SelectedAvatarIndex = 1337;
    public int SelectedAvatarIndex
    {
        get { return _SelectedAvatarIndex; }
        set { SetProperty(ref _SelectedAvatarIndex, value); }
    }
    private string _SelectedAvatar = string.Empty;
    public string SelectedAvatar
    {
        get { return _SelectedAvatar; }
        set { SetProperty(ref _SelectedAvatar, value); }
    }
    private bool _Is1Selected;
    public bool Is1Selected
    {
        get { return _Is1Selected; }
        set { SetProperty(ref _Is1Selected, value); }
    }
    private bool _Is2Selected;
    public bool Is2Selected
    {
        get { return _Is2Selected; }
        set { SetProperty(ref _Is2Selected, value); }
    }
    private bool _Is3Selected;
    public bool Is3Selected
    {
        get { return _Is3Selected; }
        set { SetProperty(ref _Is3Selected, value); }
    }
    private bool _Is4Selected;
    public bool Is4Selected
    {
        get { return _Is4Selected; }
        set { SetProperty(ref _Is4Selected, value); }
    }
    public bool IsAutoAnimationRunning { get; set; }

    public bool IsUserInteractionRunning { get; set; }

    public PopUpSelectAvatarViewModel()
    {
        int i = 1;
        Items = new ObservableCollection<object>
        {
            new { Img1 = $"avatar_0{i++}.png", Img2 = $"avatar_0{i++}.png", Img3 = $"avatar_0{i++}.png", Img4 = $"avatar_0{i++}.png" },
            new { Img1 = $"avatar_0{i++}.png", Img2 = $"avatar_0{i++}.png", Img3 = $"avatar_0{i++}.png", Img4 = $"avatar_0{i++}.png" },
            new { Img1 = $"avatar_0{i++}.png", Img2 = $"avatar_{i++}.png", Img3 = $"avatar_{i++}.png", Img4 = $"avatar_{i++}.png" },
            new { Img1 = $"avatar_{i++}.png", Img2 = $"avatar_{i++}.png", Img3 = $"avatar_{i++}.png", Img4 = $"avatar_{i++}.png" },
            new { Img1 = $"avatar_{i++}.png", Img2 = $"avatar_{i++}.png", Img3 = $"avatar_{i++}.png", Img4 = $"avatar_{i++}.png" },
            new { Img1 = $"avatar_{i++}.png", Img2 = $"avatar_{i++}.png", Img3 = $"avatar_{i++}.png", Img4 = $"avatar_{i++}.png" }
        };
        PanPositionChangedCommand = new Command(v =>
        {
            if (IsAutoAnimationRunning || IsUserInteractionRunning)
            {
                return;
            }

            var index = CurrentIndex + (bool.Parse(v.ToString()) ? 1 : -1);
            if (index < 0 || index >= Items.Count)
            {
                return;
            }
            CurrentIndex = index;
        });
        GoToLastCommand = new Command(() =>
        {
            CurrentIndex = Items.Count - 1;
        });
    }


    public ICommand PanPositionChangedCommand { get; }

    public ICommand GoToLastCommand { get; }


    private ICommand _OnImageClicked;
    public ICommand OnImageClicked
    {
        get { return _OnImageClicked = _OnImageClicked ?? new Command<string>(OnImageClickedAction); }
    }
    private void OnImageClickedAction(string obj)
    {
        SelectedAvatar = obj;
        SelectedAvatarIndex = CurrentIndex;
        string temp = obj.Substring(7, 2);//33,2
        int.TryParse(temp, out int slot);
        Is1Selected = (slot % 4 == 1);
        Is2Selected = (slot % 4 == 2);
        Is3Selected = (slot % 4 == 3);
        Is4Selected = (slot % 4 == 0);
        //await App.Current.MainPage.DisplayAlert("test", obj, "OK");
    }

    private void UpdateUI(int index)
    {
        if (index == SelectedAvatarIndex)
        {
            string temp = SelectedAvatar.Substring(7, 2);//33,2
            int.TryParse(temp, out int slot);
            Is1Selected = (slot % 4 == 1);
            Is2Selected = (slot % 4 == 2);
            Is3Selected = (slot % 4 == 3);
            Is4Selected = (slot % 4 == 0);
        }
        else
        {
            Is1Selected = false;
            Is2Selected = false;
            Is3Selected = false;
            Is4Selected = false;
        }
    }
}