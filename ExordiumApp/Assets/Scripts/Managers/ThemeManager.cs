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
    public Theme Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            ApplyThemeRecursive(transform);
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
        Theme = _lightTheme;
    }

    public void ApplyThemeRecursive(Transform parent)
    {
        if ((parent.name.Contains("Panel") || parent.name.Contains("Overlay")) 
            && parent.TryGetComponent<Image>(out Image background))
        {
            background.color = Theme.panelBackgroundColor;

            if (parent.name.Contains("Main") || parent.name.Contains("Transparent"))
            {
                Color color = background.color;
                color.a = 0f;
                background.color = color;
            }
        }
        else if (parent.name.Contains("Checkmark"))
        {
            parent.GetComponent<Image>().color = Theme.textColor;
        }
        else if (parent.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
        {
            text.color = Theme.textColor;

            // Adjusting for Input Field Text
            if (text.name.Contains("Input"))
            {
                text.color = Theme == _darkTheme ? Color.white : Color.black;
            }
        }
        else if (parent.TryGetComponent<Button>(out Button button)) 
        {
            ColorBlock colors = button.colors;  
            colors.normalColor = Theme.buttonBackgroundColor;
            button.colors = colors;
        }
        else if (parent.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.image.color = Theme.buttonBackgroundColor;
        }
        
        foreach(Transform child in parent)
        {
            ApplyThemeRecursive(child);
        }
    }
}
