using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ang7.Models;

public class TeacherFile : INotifyPropertyChanged
{
    private string name;
    private int parentfolderid;

    public int ID { get; set; }
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
    public int TypeID { get; set; }
    public bool IsFolder => TypeID == -1;
    public string Image => (IsFolder && !string.IsNullOrEmpty(Link))? Link : GlobalFunc.GetFileIcon(TypeID);
    public int ParentFolderID
    {
        get => parentfolderid;
        set
        {
            if (parentfolderid != value)
            {
                parentfolderid = value;
                OnPropertyChanged();
            }
        }
    }
    public string Link { get; set; }
    public DateTime CDate { get; set; }
    public bool IsSecured { get; set; }

    private Color _borderColor = Color.FromRgb(246, 248, 249);

    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            OnPropertyChanged(nameof(BorderColor));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}