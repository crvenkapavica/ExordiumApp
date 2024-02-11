using UnityEngine;

public class UserData
{
    public static UserData Instance { get; private set; } = new UserData();
    public string Username { get; set; }

    public bool IsLoggedIn { get; private set; }

    public void UpdateLoginStatus(bool bIsLoggedIn, string username)
    {
        IsLoggedIn = bIsLoggedIn;
        Username = username;
    }

    public void SavePlayerPrefs()
    {
        if (IsLoggedIn)
        {
            PlayerPrefs.SetString(Username, Username);

            string theme = ThemeManager.Instance.Theme.ThemeName;
            PlayerPrefs.SetString(Username + "_Theme", theme);

            string language = LocalizationManager.Instance.Language.ToString();
            PlayerPrefs.SetString(Username + "_Language", language);

            PlayerPrefs.Save();
        }
    }

    public void LoadPlayerPrefs()
    {
        string theme = PlayerPrefs.GetString(Username + "_Theme");
        ThemeManager.Instance.Theme =
            theme == string.Empty || theme == "Dark Theme"
            ? ThemeManager.Instance.DarkTheme
            : ThemeManager.Instance.LightTheme;
        ThemeManager.Instance.ApplyTheme(ThemeManager.Instance.Theme, true);

        string lanuage = PlayerPrefs.GetString(Username + "_Language");
        LocalizationManager.Instance.Language =
            lanuage == string.Empty || lanuage == Language.Croatian.ToString()
            ? Language.Croatian
            : Language.English;
        LocalizationManager.Instance.ApplyLocalization(
            LocalizationManager.Instance.Language, true
        );

        //var favorites = new List<int>();

        //string favoritesString = PlayerPrefs.GetString(userName + "_Favorites", "");
        //if (!string.IsNullOrEmpty(favoritesString))
        //{
        //    favorites = favoritesString.Split(',').Select(int.Parse).ToList();
        //}

        //bool toggle1 = PlayerPrefs.GetInt(userName + "_Toggle1", 0) == 1;
        //bool toggle2 = PlayerPrefs.GetInt(userName + "_Toggle2", 0) == 1;
    }
}