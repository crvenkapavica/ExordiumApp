using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

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

        AssignButtonEvents();
    }

    private void Start()
    {
        AssignLanguageToggleEvents();
    }

    private void AssignButtonEvents()
    {
        foreach (var panelMapping in UIManager.Instance.PanelMappings)
        {
            GameObject panel = panelMapping.panelObject;
            panel.SetActive(true);

            // Handle the disabled Logout button
            if (panelMapping.panelType == PanelType.Account)
            {
                GameObject logout = panel.transform.Find("LoggedInPanelTransparent/Logout").gameObject;
                logout.SetActive(true);
                logout.GetComponent<Button>().onClick.AddListener(() => ButtonClicked_Logout());
                logout.SetActive(false);
            }

            Button[] buttons = panel.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                switch (button.name)
                {
                    case "Items":
                        button.onClick.AddListener(() => ButtonClicked_Items());            break;
                    case "Category":
                        button.onClick.AddListener(() => ButtonClicked_Category());         break;
                    case "Account":
                        button.onClick.AddListener(() => ButtonClicked_Account());          break;
                    case "Retailer":
                        button.onClick.AddListener(() => ButtonClicked_Retailer());         break;
                    case "Favorites":
                        button.onClick.AddListener(() => ButtonClicked_Favorites());        break;
                    case "Settings":
                        button.onClick.AddListener(() => ButtonClicked_Settings());         break;
                    case "Register":
                        button.onClick.AddListener(() => ButtonClicked_Register(button));   break;
                    case "Login":
                        button.onClick.AddListener(() => ButtonClicked_Login(button));      break;
                    case "Language":
                        button.onClick.AddListener(() => ButtonClicked_Language());         break;
                    case "Theme":
                        button.onClick.AddListener(() => ButtonClicked_Theme());            break;
                    case "RetryContinue":
                        button.onClick.AddListener(() => MessageBoxClicked());              break;
                    case "DarkTheme":
                        button.onClick.AddListener(() => ButtonClicked_DarkTheme());        break;
                    case "LightTheme":
                        button.onClick.AddListener(() => ButtonClicked_LightTheme());       break;
                    case "Confirm":
                        button.onClick.AddListener(() => ButtonClicked_Confirm());          break;
                    case "Cancel":
                        button.onClick.AddListener(() => ButtonClicked_Cancel());           break;


                    default:
                        break;
                }
            }

            if (panelMapping.panelType != PanelType.Navigation && panelMapping.panelType != PanelType.Items)
            {
                panel.SetActive(false);
            }
        }
    }

    private void AssignLanguageToggleEvents()
    {
        UIManager.Instance.LanguageToggles[(int)LanguageToggle.Croatian]
            .onValueChanged.AddListener((isOn) =>
            {
                if (isOn) { LocalizationManager.Instance.ApplyLocalization(Language.Croatian, false); }
            });

        UIManager.Instance.LanguageToggles[(int)LanguageToggle.English]
            .onValueChanged.AddListener((isOn) =>
            {
                if (isOn) { LocalizationManager.Instance.ApplyLocalization(Language.English, false); }
            });
    }

    // NAVIGATION
    private void ButtonClicked_Items()
    {
        UIManager.Instance.ShowPanel(PanelType.Items);
    }

    private void ButtonClicked_Category()
    {
        UIManager.Instance.ShowPanel(PanelType.Category);
    }

    private void ButtonClicked_Account()
    {
        UIManager.Instance.ShowPanel(PanelType.Account);
    }

    private void ButtonClicked_Retailer()
    {
        UIManager.Instance.ShowPanel(PanelType.Retailer);
    }

    private void ButtonClicked_Favorites()
    {
        UIManager.Instance.ShowPanel(PanelType.Favorites);
    }

    private void ButtonClicked_Settings()
    {
        UIManager.Instance.ShowPanel(PanelType.Settings);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////
    
    // ACCOUNT
    private void ButtonClicked_Register(Button button)
    {
        Transform parent = button.transform.parent.parent.parent;
        TextMeshProUGUI emailInput = parent.Find("EmailPanel/Input/TextArea/EmailInput").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI passwordInput = parent.Find("PasswordPanel/Input/TextArea/PasswordInput").GetComponent<TextMeshProUGUI>();

        RegisterManager.Instance.AttemptRegister(emailInput.text, passwordInput.text);

        parent.Find("EmailPanel/Input").GetComponent<TMP_InputField>().text = "";
        parent.Find("PasswordPanel/Input").GetComponent<TMP_InputField>().text = "";
    }
    
    private void ButtonClicked_Login(Button button)
    {
        Transform parent = button.transform.parent.parent.parent;
        TextMeshProUGUI emailInput = parent.Find("EmailPanel/Input/TextArea/EmailInput").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI passwordInput = parent.Find("PasswordPanel/Input/TextArea/PasswordInput").GetComponent<TextMeshProUGUI>();

        LoginManager.Instance.AttemptLogin(emailInput.text, passwordInput.text);

        parent.Find("EmailPanel/Input").GetComponent<TMP_InputField>().text = "";
        parent.Find("PasswordPanel/Input").GetComponent<TMP_InputField>().text = "";
    }

    private void ButtonClicked_Logout()
    {
        LoginManager.Instance.Logout();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////
    
    // SETTINGS
    private void ButtonClicked_Language()
    {
        UIManager.Instance.ShowOverlay(
            UIManager.Instance.PanelMappings[(int)PanelType.Language].panelObject
        );
    }

    private void ButtonClicked_Theme()
    {
        UIManager.Instance.ShowOverlay(
            UIManager.Instance.PanelMappings[(int)PanelType.Theme].panelObject
        );
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////

    // OVERLAYS
    private void MessageBoxClicked()
    {
        UIManager.Instance.HideOverlays();
    }

    private void ButtonClicked_DarkTheme()
    {
        ThemeManager.Instance.ApplyTheme(ThemeManager.Instance.DarkTheme, false);
    }

    private void ButtonClicked_LightTheme()
    {
        ThemeManager.Instance.ApplyTheme(ThemeManager.Instance.LightTheme, false);
    }

    private void ButtonClicked_Confirm()
    {
        LocalizationManager.Instance.ApplyLocalization(LocalizationManager.Instance.Language, true);
        ThemeManager.Instance.ApplyTheme(ThemeManager.Instance.Theme, true);
        UIManager.Instance.ThemeName.text = ThemeManager.Instance.ThemeName;

        UIManager.Instance.HideOverlays();
    }

private void ButtonClicked_Cancel()
    {
        LocalizationManager.Instance.ApplyLocalization(Language.Null, true);
        ThemeManager.Instance.ApplyTheme(null, true);
        UIManager.Instance.HideOverlays();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////
}


//    if (UserData.Instance.IsLoggedIn)
//{
//    // Save to PlayerPrefs if logged in
//}