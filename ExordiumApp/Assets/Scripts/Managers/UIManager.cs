using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PanelMaping
{
    public PanelType panelType;
    public GameObject panelObject;
}

public enum PanelType
{
    Navigation,
    Items,
    Category,
    Account,
    Retailer,
    Favorites,
    Settings,
    Fetching,
    MessageBox,
    Language,
    Theme
}

public enum EMessageBoxResponse
{
    Response_OK,
    Response_Email,
    Response_Credentials,
    Response_Welcome,
}

public enum LanguageToggle
{
    Croatian,
    English
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _activeMainPanel;
    public GameObject ActiveMainPanel
    {
        get => _activeMainPanel;
        set
        {
            _activeMainPanel.SetActive(false);
            _activeMainPanel = value;
            _activeMainPanel.SetActive(true);

            if (!_bIsRetailersInitialzed 
                && _activeMainPanel == _panelMappings[(int)PanelType.Retailer].panelObject)
            {
                DisplayManager.Instance.UpdateRetailerDisplay(
                    ApplicationData.Instance.Retailers
                );

                _bIsRetailersInitialzed = true;
            }
            else if (!_bIsCategoriesInitialized
                && _activeMainPanel == _panelMappings[(int)PanelType.Category].panelObject)
            {
                DisplayManager.Instance.UpdateCategoryDisplay(
                    ApplicationData.Instance.Categories
                );

                _bIsCategoriesInitialized = true;
            }

            if (_activeMainPanel == _panelMappings[(int)PanelType.Favorites].panelObject)
            {
                DisplayManager.Instance.UpdateFavoritesDisplay();
            }
        }
    }

    public HashSet<int> Favorites { get; set; } = new();

    [SerializeField] private List<PanelMaping> _panelMappings;
    public List<PanelMaping> PanelMappings => _panelMappings;

    [SerializeField] private GameObject _outterOverlayPanel;
    [SerializeField] private GameObject _innerOverlayPanel;
    [SerializeField] private GameObject[] _overlays;

    [SerializeField] private TextMeshProUGUI _languageName;
    public TextMeshProUGUI LanguageName
    {
        get => _languageName;
        set => _languageName = value;
    }

    [SerializeField] private TextMeshProUGUI _themeName;
    public TextMeshProUGUI ThemeName
    {
        get => _themeName;
        set => _themeName = value;
    }

    [SerializeField] private Toggle[] _languageToggles;
    public Toggle[] LanguageToggles => _languageToggles;

    private const float FADE_TIME = 0.25f;

    private bool _bIsCategoriesInitialized;
    private bool _bIsRetailersInitialzed;

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

    public void ShowOverlay(GameObject messageBox, EMessageBoxResponse response)
    {
        if (!messageBox.name.Contains("MessageBox")) return;

        _outterOverlayPanel.SetActive(true);

        Transform button;
        if (!(button = messageBox.transform.Find("InnerPanelTransparent/RetryContinue/Retry")))
            button = messageBox.transform.Find("InnerPanelTransparent/RetryContinue/Continue");
            
        Transform message;
        if (!(message = messageBox.transform.Find("InnerPanelTransparent/Message")))
            if (!(message = messageBox.transform.Find("InnerPanelTransparent/Response_OK")))
                if (!(message = messageBox.transform.Find("InnerPanelTransparent/Response_Email")))
                    if (!(message = messageBox.transform.Find("InnerPanelTransparent/Response_Credentials")))
                        message = messageBox.transform.Find("InnerPanelTransparent/Response_Welcome");

        switch (response)
        {
            case EMessageBoxResponse.Response_OK:
                button.name = "Continue";
                message.name = "Response_OK";
                break;
            case EMessageBoxResponse.Response_Email:
                button.name = "Retry";
                message.name = "Response_Email";
                break;
            case EMessageBoxResponse.Response_Credentials:
                button.name = "Retry";
                message.name = "Response_Credentials";
                break;
            case EMessageBoxResponse.Response_Welcome:
                button.name = "Continue";
                message.name = "Response_Welcome";
                break;

            default:
                break;
        }

        LocalizationManager.Instance.LocalizeTextRecursive(
            messageBox.transform, LocalizationManager.Instance.Language
        );

        AnchorOverlayMessageBox();
        _innerOverlayPanel.SetActive(true);
        messageBox.SetActive(true);
        StartCoroutine(FadeIn(messageBox.GetComponent<CanvasGroup>()));
    }

    public void ShowOverlay(GameObject overlay)
    {
        _outterOverlayPanel.SetActive(true);      

        if (overlay.name.Contains("Language") || overlay.name.Contains("Theme"))
        {
            AnchorOverlayLanguageTheme();
        }
        else if (overlay.name.Contains("Fetching"))
        {
            AnchorOverlayFetching();
        }

        _innerOverlayPanel.SetActive(true);
        overlay.SetActive(true);

        StartCoroutine(FadeIn(overlay.GetComponent<CanvasGroup>()));
    }
        
    public void HideOverlays()
    {
        foreach (var overlay in _overlays)
        {
            if (overlay.activeSelf)
            {
                StartCoroutine(
                    FadeOut(overlay.GetComponent<CanvasGroup>(), () =>
                    {
                        overlay.SetActive(false);
                        _innerOverlayPanel.SetActive(false);
                        _outterOverlayPanel.SetActive(false);
                    })
                );
                break;
            }
        }
    }

    public void ShowPanel(PanelType panelType)
    {
        if (_panelMappings[(int)panelType].panelObject == ActiveMainPanel) return;

        ActiveMainPanel = _panelMappings[(int)panelType].panelObject;
    }

    public void ToggleAccountPanel(bool bToggle, string username = "")
    {
        Transform panel = UIManager.Instance.ActiveMainPanel.transform;
        panel.Find("EmailPanel").gameObject.SetActive(bToggle);
        panel.Find("PasswordPanel").gameObject.SetActive(bToggle);
        panel.Find("ButtonPanel").gameObject.SetActive(bToggle);

        panel.Find("LoggedInPanelTransparent/Logout").gameObject.SetActive(!bToggle);
        var usernameText = panel.Find("LoggedInPanelTransparent/Username").GetComponent<TextMeshProUGUI>();
        usernameText.text = username;
        usernameText.gameObject.SetActive(!bToggle);
    }

    private void AnchorOverlayMessageBox()
    {
        var rect = _outterOverlayPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.075f, 0.425f);
        rect.anchorMax = new Vector2(0.925f, 0.575f);
    }

    private void AnchorOverlayLanguageTheme()
    {
        var rect = _outterOverlayPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.3f);
        rect.anchorMax = new Vector2(1, 0.7f);
    }

    private void AnchorOverlayFetching()
    {
        var rect = _outterOverlayPanel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.25f);
        rect.anchorMax = new Vector2(1, 0.75f);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, FADE_TIME));
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, Action onComplete)
    {
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, FADE_TIME));
        onComplete?.Invoke();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float lerpTime)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted;
        float percentageComplete = 0f;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            canvasGroup.alpha = currentValue;

            yield return new WaitForEndOfFrame();
        }
    }
}