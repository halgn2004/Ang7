using Ang7.Helpers;
using Newtonsoft.Json;

namespace Ang7.Models;

public class Teacher
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public int NameFontSize => Name.Length >= 18 ? 10 : Name.Length >= 12 ? 12 : 16;
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PP { get; set; }
    public string CP { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime CDate { get; set; }
    public string Bio { get; set; }
    public bool IsPublic { get; set; }
    public List<string> Subjects { get; set; }
    public int SubjectsHeight => Subjects.Count > 1 ? (Subjects.Count * 40)-5 : (Subjects.Count * 35);
    public List<TeacherFile> Files { get; set; }
}
