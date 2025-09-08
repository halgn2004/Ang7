using Ang7.Models;
using Newtonsoft.Json;

namespace Ang7.Helpers;

public static class Settings
{
    public static bool IsTheAppUpdated
    {
        get => Preferences.Get(nameof(IsTheAppUpdated), true);
        set => Preferences.Set(nameof(IsTheAppUpdated), value);
    }
    public static DateTime LastUpdateDate
    {
        get
        {
            var storedValue = Preferences.Get(nameof(LastUpdateDate), string.Empty);
            return DateTime.TryParse(storedValue, out var date) ? date : DateTime.MinValue;
        }
        set => Preferences.Set(nameof(LastUpdateDate), value.ToString("o")); // ISO 8601 format
    }

    /**/
    public static bool GeneralSettings_KeepMeLoggedIn
    {
        get => Preferences.Get(nameof(GeneralSettings_KeepMeLoggedIn), false);
        set => Preferences.Set(nameof(GeneralSettings_KeepMeLoggedIn), value);
    }

    public static bool GeneralSettings_SkipIntro
    {
        get => Preferences.Get(nameof(GeneralSettings_SkipIntro), false);
        set => Preferences.Set(nameof(GeneralSettings_SkipIntro), value);
    }
    public static bool GeneralSettings_SkipSS
    {
        get => Preferences.Get(nameof(GeneralSettings_SkipSS), false);
        set => Preferences.Set(nameof(GeneralSettings_SkipSS), value);
    }

    public static string Usual_NickName
    {
        get => Preferences.Get(nameof(Usual_NickName), string.Empty);
        set => Preferences.Set(nameof(Usual_NickName), value);
    }

    public static string LoggedInUser_User
    {
        get => Preferences.Get(nameof(LoggedInUser_User), string.Empty);
        set => Preferences.Set(nameof(LoggedInUser_User), value);
    }

    public static void KeepMeLoggedIn_SetUser(User lk)
    {
        GeneralSettings_KeepMeLoggedIn = true;
        LoggedInUser_User = JsonConvert.SerializeObject(lk);
    }

    public static User? KeepMeLoggedIn_GetUser()
    {
        string userData = LoggedInUser_User;
        return !string.IsNullOrEmpty(userData)
            ? JsonConvert.DeserializeObject<User>(userData)
            : null;
    }

    /*CreateCopouns*/
    public static string PWPatters_Pattern
    {
        get => Preferences.Get(nameof(PWPatters_Pattern), "1000");
        set => Preferences.Set(nameof(PWPatters_Pattern), value);
    }
    public static int PWPatters_CodeLenght
    {
        get => Preferences.Get(nameof(PWPatters_CodeLenght), 6);
        set => Preferences.Set(nameof(PWPatters_CodeLenght), value);
    }
    public static int PWPatters_CopounsCount
    {
        get => Preferences.Get(nameof(PWPatters_CopounsCount), 1);
        set => Preferences.Set(nameof(PWPatters_CopounsCount), value);
    }
    public static DateTime PWPatters_ValidTo
    {
        get => Preferences.Get(nameof(PWPatters_ValidTo), DateTime.Today);
        set => Preferences.Set(nameof(PWPatters_ValidTo), value);
    }


}
