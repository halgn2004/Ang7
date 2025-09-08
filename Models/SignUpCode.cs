using Ang7.Helpers;
using Newtonsoft.Json;

namespace Ang7.Models;

public class SignUpCode
{
    public int ID { get; set; }
    public int UserType { get; set; }
    public string Code { get; set; }
    public int TeacherID { get; set; }
    public int UserID { get; set; }
    public int Used { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime CDate { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime UDate { get; set; }

}
