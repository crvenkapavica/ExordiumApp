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

    public HashSet<string> CategoryFilter { get; set; } = new();
    public HashSet<string> RetailerFilter { get; set; } = new();

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
        return favorites;
    }

    public void SaveFavorites(HashSet<int> favorites)
    {
        if (IsLoggedIn)
        {
            var favoritesString = string.Join(",", favorites.Select(id => id.ToString()).ToArray());
            PlayerPrefs.SetString(Username + "_Favorites", favoritesString);
            PlayerPrefs.Save();
        }
    }

    public void SaveFilters()
    {
        if (IsLoggedIn)
        {
            var filtersString = string.Join(",", RetailerFilter.ToArray());
            PlayerPrefs.SetString(Username + "_RetailerFilter", filtersString);
            filtersString = string.Join(",", CategoryFilter.ToArray());
            PlayerPrefs.SetString(Username + "_CategoryFilter", filtersString);
            PlayerPrefs.Save();
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


        var retailerFiltersString = PlayerPrefs.GetString(Username + "_RetailerFilter", "");
        RetailerFilter = new HashSet<string>(
            retailerFiltersString.Split(',')
            .Where(id => !string.IsNullOrEmpty(id))
        );

        var categoryFiltersString = PlayerPrefs.GetString(Username + "_CategoryFilter", "");
        CategoryFilter = new HashSet<string>(
            categoryFiltersString.Split(',')
            .Where(id => !string.IsNullOrEmpty(id))
        );

        DisplayManager.Instance.ApplyAllItemFilters();
    }
}