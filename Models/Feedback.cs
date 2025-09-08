namespace Ang7.Models;

public class Feedback
{
    public int ID { get; set; }
    public string UserPhone { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int TitleID { get; set; }
    public string Info { get; set; }
    public DateTime CDate { get; set; }
    public bool Readed { get; set; }
    public string Replied { get; set; }
    public DateTime ReplyDate { get; set; }


    public string Dayname => GlobalFunc.ConvertDaysToAr(CDate.DayOfWeek.ToString());
    public string Day => GlobalFunc.ConvertNumberToAr(CDate.Day.ToString());
    public string Month => GlobalFunc.ConvertmonthToAr(CDate.Month);
    public string Hour => (((CDate.Hour % 12) == 0) ? "۱۲" : GlobalFunc.ConvertNumberToAr((CDate.Hour % 12).ToString()));
    public string min => $":{GlobalFunc.ConvertNumberToAr(CDate.Minute.ToString())} ";
    public string ampm => (CDate.Hour > 12) ? "مساء" : "صباحا";

    public string RDayname => GlobalFunc.ConvertDaysToAr(ReplyDate.DayOfWeek.ToString());
    public string RDay => GlobalFunc.ConvertNumberToAr(ReplyDate.Day.ToString());
    public string RMonth => GlobalFunc.ConvertmonthToAr(ReplyDate.Month);
    public string RHour => (((ReplyDate.Hour % 12) == 0) ? "۱۲" : GlobalFunc.ConvertNumberToAr((ReplyDate.Hour % 12).ToString()));
    public string Rmin => $":{GlobalFunc.ConvertNumberToAr(ReplyDate.Minute.ToString())} ";
    public string Rampm => (ReplyDate.Hour > 12) ? "مساء" : "صباحا";


    public string Title => Consts.fbstitle[TitleID];
    public int infohieght => 30 + (Info.Length/ 50) * 20;

}
