using RGPopup.Maui.Services;
using System.Windows.Input;

namespace Ang7.ViewModels;

public class PopUpLoadingViewModel : BaseViewModel
{
    private readonly INavigation Navigation;
    public  bool IsLoading = false;

    public PopUpLoadingViewModel(INavigation _navigation)
    {
        Navigation = _navigation;
        AIVisible = true;
        Labeltxt = buttxt = "LegendKnight";
        LabelVisible = ButVisible = false;
        FramePadding = new Thickness(10,10,10,10);
        IsLoading = true;
    }
    public void ShowMsg(string msg, string bt)
    {
        AIVisible = false;
        Labeltxt = msg;
        buttxt = bt;
        LabelVisible = ButVisible = true;
        FramePadding = new Thickness(50, 50, 50, 50);
        IsLoading = false;
    }
    public void Done()
    {
        IsLoading = false;
        PopupNavigation.Instance.PopAsync();
    }

    private bool _AIVisible;
    public bool AIVisible
    {
        get { return _AIVisible; }
        set { SetProperty(ref _AIVisible, value); }
    }


    private Thickness _FramePadding;
    public Thickness FramePadding
    {
        get { return _FramePadding; }
        set { SetProperty(ref _FramePadding, value); }
    }
    private string _Labeltxt;
    public string Labeltxt
    {
        get { return _Labeltxt; }
        set { SetProperty(ref _Labeltxt, value); }
    }
    private bool _LabelVisible;
    public bool LabelVisible
    {
        get { return _LabelVisible; }
        set { SetProperty(ref _LabelVisible, value); }
    }

    private string _buttxt;
    public string buttxt
    {
        get { return _buttxt; }
        set { SetProperty(ref _buttxt, value); }
    }
    private bool _ButVisible;
    public bool ButVisible
    {
        get { return _ButVisible; }
        set { SetProperty(ref _ButVisible, value); }
    }
    private ICommand _ButClickCommand;
    public ICommand ButClickCommand
    {
        get { return _ButClickCommand = _ButClickCommand ?? new Command(ButClickAction); }
    }
    private void ButClickAction(object obj)
    {
        IsLoading = false;
        PopupNavigation.Instance.PopAsync();
    }

}