using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class UserData
{
    public static UserData Instance { get; private set; } = new UserData();
    public string Username { get; private set; }

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
            }
        }
    }

    public void UpdateLoginStatus(bool bIsLoggedIn, string username = "")
    {
        Username = username;
        IsLoggedIn = bIsLoggedIn;
    }

    public void SavePlayerPrefs()
    {
        if (_bIsLoggedIn)
        {
            string theme = ThemeManager.Instance.Theme.ThemeName;
            PlayerPrefs.SetString(Username + "_Theme", theme);

            string language = LocalizationManager.Instance.Language.ToString();
            PlayerPrefs.SetString(Username + "_Language", language);

            PlayerPrefs.Save();
        }
    }

    private void LoadPlayerPrefs(string username)
    {
        string userName = PlayerPrefs.GetString(username);

        string theme = PlayerPrefs.GetString(userName + "_Theme");
        ThemeManager.Instance.ApplyTheme(
            theme == "" || theme == "_DarkTheme" 
            ? ThemeManager.Instance.DarkTheme
            : ThemeManager.Instance.LightTheme, true);

        string lanuage = PlayerPrefs.GetString(userName + "_Language");
        LocalizationManager.Instance.Language =
            lanuage == "" || lanuage == Language.Croatian.ToString()
            ? Language.Croatian
            : Language.English;
    }
}