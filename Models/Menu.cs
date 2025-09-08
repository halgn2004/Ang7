using Ang7.ViewModels;

namespace Ang7.Models;

public class Menu : BaseViewModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    private string _Badge;
    public string Badge
    {
        get { return _Badge; }
        set { SetProperty(ref _Badge, value); }
    }
}
