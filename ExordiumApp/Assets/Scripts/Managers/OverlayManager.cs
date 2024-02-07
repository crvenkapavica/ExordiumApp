using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance { get; private set; }

    [SerializeField] private GameObject _outterOverlayPanel;
    [SerializeField] private GameObject _innerOverlayPanel;
    [SerializeField] private GameObject[] _overlays;

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

    public void ShowOverlay(GameObject overlay)
    {
        _outterOverlayPanel.SetActive(true);

        //for mBOX
        var rect = _outterOverlayPanel.GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y - Screen.height * 0.9f);
        //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        rect.anchorMin = new Vector2(0.075f, 0.425f);
        rect.anchorMax = new Vector2(0.925f, 0.575f);
        //rect.sizeDelta = new Vector2(1, Screen.height * 0.4f);

        _innerOverlayPanel.SetActive(true);
        foreach (var o in _overlays)
        {
            o.SetActive(o == overlay);
        }
    }

    public void HideOverlays()
    {
        foreach (var overlay in _overlays)
        {
            overlay.SetActive(false);
        }
        _innerOverlayPanel.SetActive(false);
        _outterOverlayPanel.SetActive(false);
    }
}