using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class UserData
{
    public static UserData Instance { get; private set; } = new UserData();
    public string Username { get; set; }

    private bool _bIsLoggedIn;
    public bool IsLoggedIn
    {
        get => _bIsLoggedIn;
        set
        {
            _bIsLoggedIn = value;
            if (_bIsLoggedIn)
            {
                LoadPlayerPrefs(Username);
            }
            else
            {
                SavePlayerPrefs();
                ApplicationData.Instance.LoadDefaultPrefs();
                Username = string.Empty;
            }
        }
    }

    public void UpdateLoginStatus(bool bIsLoggedIn, string username)
    {
        Username = username;
        IsLoggedIn = bIsLoggedIn;
    }

    public void SavePlayerPrefs()
    {
        if (_bIsLoggedIn)
        {
            Debug.Log("SAVING USERNAME: " + Username);

            PlayerPrefs.SetString(Username, Username);

            string theme = ThemeManager.Instance.Theme.ThemeName;
            PlayerPrefs.SetString(Username + "_Theme", theme);

            Debug.Log("SAVING theme " + theme);

            string language = LocalizationManager.Instance.Language.ToString();
            PlayerPrefs.SetString(Username + "_Language", language);

            Debug.Log("Saving lang" + language);

            PlayerPrefs.Save();
        }
    }

    private void LoadPlayerPrefs(string username)
    {
        string userName = PlayerPrefs.GetString(username);

        Debug.Log("loaded username: " + userName);

        string theme = PlayerPrefs.GetString(userName + "_Theme");
        ThemeManager.Instance.ApplyTheme(
            theme == string.Empty || theme == "DarkTheme" 
            ? ThemeManager.Instance.DarkTheme
            : ThemeManager.Instance.LightTheme, true);

        Debug.Log("loaded theme" + theme);
        Debug.Log("loaded Instance.Theme" + ThemeManager.Instance.Theme.ToString());

        string lanuage = PlayerPrefs.GetString(userName + "_Language");
        LocalizationManager.Instance.Language =
            lanuage == string.Empty || lanuage == Language.Croatian.ToString()
            ? Language.Croatian
            : Language.English;

        Debug.Log("loaded lang" + lanuage);
        Debug.Log("loaded Instance.lan" + LocalizationManager.Instance.Language);

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