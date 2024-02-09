using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        }
    }

    [SerializeField] private List<PanelMaping> _panelMappings = new();
    public List<PanelMaping> PanelMappings => _panelMappings;

    [SerializeField] private GameObject _outterOverlayPanel;
    [SerializeField] private GameObject _innerOverlayPanel;
    [SerializeField] private GameObject[] _overlays;

    public enum EMessageBoxResponse
    {
        Response_OK,
        Response_Email,
        Response_Credentials,
        Response_Welcome,
    }

    private enum EOverlays
    {
        MessageBox = 0,
        Language,
        Theme,
        Fetching
    }

    private const float FADE_TIME = 0.25f;

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

        var buttonText = messageBox.transform.Find("InnerPanelTransparent/RetryContinue/Retry");
        if (buttonText == null)
            buttonText = messageBox.transform.Find("InnerPanelTransparent/Continue");

        var messageText = messageBox.transform.Find("InnerPanelTransparent/Message");
        if (messageText == null)
            messageText = messageBox.transform.Find("InnerPanelTransparent/Response_OK");
        if (messageText == null)
            messageText = messageBox.transform.Find("InnerPanelTransparent/Response_Email");
        if (messageText == null)
            messageText = messageBox.transform.Find("InnerPanelTransparent/Response_Credentials");
        if (messageText == null)
            messageText = messageBox.transform.Find("InnerPanelTransparent/Response_Welcome");

        switch (response)
        {
            case EMessageBoxResponse.Response_OK:
                buttonText.name = "Continue";
                messageText.name = "Response_OK";
                break;
            case EMessageBoxResponse.Response_Email:
                buttonText.name = "Retry";
                messageText.name = "Response_Email";
                break;
            case EMessageBoxResponse.Response_Credentials:
                buttonText.name = "Retry";
                messageText.name = "Response_Credentials";
                break;
            case EMessageBoxResponse.Response_Welcome:
                buttonText.name = "Continue";
                messageText.name = "Response_Welcome";
                break;
            default:
                break;
        }

        LocalizationManager.Instance.LocalizeTextRecursive(messageBox.transform);

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
        TextMeshProUGUI usernameText = panel.Find("LoggedInPanelTransparent/Username").GetComponent<TextMeshProUGUI>();
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