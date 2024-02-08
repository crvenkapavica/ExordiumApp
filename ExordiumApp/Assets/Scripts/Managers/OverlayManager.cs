using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance { get; private set; }

    [SerializeField] private GameObject _outterOverlayPanel;
    [SerializeField] private GameObject _innerOverlayPanel;
    [SerializeField] private GameObject[] _overlays;

    public enum EMessageBoxResponse
    {
        Response_OK,
        Response_Email,
        Response_Credentials
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
        //if (!messageBox.name.Contains("MessageBox")) return;

        //var buttonText = messageBox.transform.Find("InnerPanelTransparent/Button/Retry");
        //var messageText = messageBox.GetComponent<TextMeshProUGUI>();

        //switch (response)
        //{
        //    case EMessageBoxResponse.Response_OK:
        //        button.name = "Continue";
        //        message.name = "Response_OK";
        //        break;
        //    case EMessageBoxResponse.Response_Email:
        //        button.name = "Retry";
        //        message.name = "Response_Email";
        //        break;
        //    case EMessageBoxResponse.Response_Credentials:
        //        button.name = "Retry";
        //        message.name = "Response_Credentials";
        //        break;
        //}
    }

    public void ShowOverlay(GameObject overlay)
    {
        _outterOverlayPanel.SetActive(true);
        var rect = _outterOverlayPanel.GetComponent<RectTransform>();

        StartCoroutine(FadeIn(overlay.GetComponent<CanvasGroup>()));

        // MessageBox panel - smallest panel - 15%
        if (overlay.name.Contains("MessageBox"))
        {
            rect.anchorMin = new Vector2(0.075f, 0.425f);
            rect.anchorMax = new Vector2(0.925f, 0.575f);
        }
        // Theme and Language panels - 40%
        else if (overlay.name.Contains("Language") || overlay.name.Contains("Theme"))
        {
            rect.anchorMin = new Vector2(0, 0.3f);
            rect.anchorMax = new Vector2(1, 0.7f);
        }
        // Fetching panel - 50%
        else if (overlay.name.Contains("Fetching"))
        {
            rect.anchorMin = new Vector2(0, 0.25f);
            rect.anchorMax = new Vector2(1, 0.75f);
        }

        _innerOverlayPanel.SetActive(true);
        overlay.SetActive(true);
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