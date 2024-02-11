using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UserData
{
    public static UserData Instance { get; private set; } = new UserData();

    public string Username { get; set; }

    public bool IsLoggedIn { get; private set; }

    public HashSet<int> Favorites { get; set; } = new();

    public HashSet<string> DiabledCategories { get; set; } = new();
    public HashSet<string> DisabledRetailers { get; set; } = new();

    public void UpdateLoginStatus(bool bIsLoggedIn, string username)
    {
        IsLoggedIn = bIsLoggedIn;
        Username = username;
    }

    public HashSet<int> GetFavorites()
    {
        var favoritesString = PlayerPrefs.GetString(Username + "_Favorites", "");
        var favorites = new HashSet<int>(
            favoritesString.Split(',')
            .Where(id => !string.IsNullOrEmpty(id))
            .Select(int.Parse)
        );

        Debug.Log("[GETFAVORITES] (favoritesString) = " + favoritesString);
        Debug.Log("[GETFAVORITES} (favorites) = " +  favorites);
        return favorites;
    }

    public void SaveFavorites(HashSet<int> favorites)
    {
        if (IsLoggedIn)
        {
            var favoritesString = string.Join(",", favorites.Select(id => id.ToString()).ToArray());
            PlayerPrefs.SetString(Username + "_Favorites", favoritesString);
            PlayerPrefs.Save();

            Debug.Log("Saving Favorites : " + favoritesString);
        }
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
        ThemeManager.Instance.ApplyTheme(
            ThemeManager.Instance.Theme, true
        );

        string lanuage = PlayerPrefs.GetString(Username + "_Language");
        LocalizationManager.Instance.Language =
            lanuage == string.Empty || lanuage == Language.Croatian.ToString()
            ? Language.Croatian
            : Language.English;
        LocalizationManager.Instance.ApplyLocalization(
            LocalizationManager.Instance.Language, true
        );

        Favorites = GetFavorites();
        DisplayManager.Instance.ToggleSavedFavorites();

        //bool toggle1 = PlayerPrefs.GetInt(userName + "_Toggle1", 0) == 1;
        //bool toggle2 = PlayerPrefs.GetInt(userName + "_Toggle2", 0) == 1;
    }
}