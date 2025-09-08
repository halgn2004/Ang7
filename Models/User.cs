using Ang7.Helpers;
using Newtonsoft.Json;

namespace Ang7.Models;

public class User
{
    public int UserID { get; set; }
    public int SubUserTo { get; set; }
    public string Email { get; set; }
    public int Activated { get; set; }
    public string Token { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public int UserType { get; set; }
    public string GPS { get; set; }
    public string PP { get; set; }
    public string CP { get; set; }
    public bool AllowSS { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? CDate { get; set; }
    public string Bio { get; set; }
    public List<string> Subjects { get; set; }
    public List<TeacherFile> Files { get; set; }

    //0 numbers , 1 small letter , 2 capital letter , 3 All
    //number,upper,lower,symbol
    //public string CopounsType { get; set; }
    //public int CopounsLenth { get; set; }

}
