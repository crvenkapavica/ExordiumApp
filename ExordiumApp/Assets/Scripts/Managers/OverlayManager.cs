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