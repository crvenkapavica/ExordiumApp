using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

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

    public string ThemeName 
        => LocalizationManager.Instance.Language == Language.English 
        ? Theme.ThemeName
        : Theme == _lightTheme ? "Svijetla Tema" : "Tamna Tema";

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

        Theme = (_theme = _darkTheme);
    }

    private void ApplyThemeRecursive(Transform parent, Theme theme)
    {
        if ((parent.name.Contains("Panel") || parent.name.Contains("Overlay")) 
            && parent.TryGetComponent<Image>(out Image background))
        {
            background.color = theme.PanelBackgroundColor;

            if (parent.name.Contains("Main") || parent.name.Contains("Transparent"))
            {
                Color color = background.color;
                color.a = 0f;
                background.color = color;
            }
        }
        else if (parent.name.Contains("Checkmark"))
        {
            parent.GetComponent<Image>().color = theme.TextColor;
        }
        else if (parent.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
        {
            text.color = theme.TextColor;
        }
        else if (parent.TryGetComponent<Button>(out Button button)) 
        {
            ColorBlock colors = button.colors;  
            colors.normalColor = theme.ButtonBackgroundColor;
            button.colors = colors;
        }
        else if (parent.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.image.color = theme.ButtonBackgroundColor;
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
