using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    [SerializeField] private Theme _lightTheme;
    [SerializeField] private Theme _darkTheme;

    public Theme LightTheme => _lightTheme;
    public Theme DarkTheme => _darkTheme;

    private Theme _theme;
    private Theme _previewTheme;

    public Theme Theme
    {
        get => _theme;
        set
        {
            _previewTheme = value;
            ApplyThemeRecursive(transform, _previewTheme);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Theme = (_theme = _lightTheme);
    }

    private void ApplyThemeRecursive(Transform parent, Theme theme)
    {
        if ((parent.name.Contains("Panel") || parent.name.Contains("Overlay")) 
            && parent.TryGetComponent<Image>(out Image background))
        {
            background.color = theme.panelBackgroundColor;

            if (parent.name.Contains("Main") || parent.name.Contains("Transparent"))
            {
                Color color = background.color;
                color.a = 0f;
                background.color = color;
            }
        }
        else if (parent.name.Contains("Checkmark"))
        {
            parent.GetComponent<Image>().color = theme.textColor;
        }
        else if (parent.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
        {
            text.color = theme.textColor;

            // This might not be needed
            // Adjusting for Input Field Text
            if (text.name.Contains("Input"))
            {
                text.color = theme == DarkTheme ? Color.white : Color.black;
            }
        }
        else if (parent.TryGetComponent<Button>(out Button button)) 
        {
            ColorBlock colors = button.colors;  
            colors.normalColor = theme.buttonBackgroundColor;
            button.colors = colors;
        }
        else if (parent.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.image.color = theme.buttonBackgroundColor;
        }
        
        foreach(Transform child in parent)
        {
            ApplyThemeRecursive(child, theme);
        }
    }

    public void ApplyTheme(Theme theme, bool bPermanent)
    {
        Theme = bPermanent 
            ? _theme = (theme == null ? _theme : _previewTheme) 
            : _previewTheme = theme;
    }       

    public void ApplyThemeLocal(Transform parent)
    {
        ApplyThemeRecursive(parent, Theme);
    }
}
