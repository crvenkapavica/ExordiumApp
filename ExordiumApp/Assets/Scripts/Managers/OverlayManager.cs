using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance { get; private set; }

    [SerializeField] private GameObject _overlayPanel;
    [SerializeField] private GameObject[] overlays;

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
        _overlayPanel.SetActive(true);
        foreach (var o in overlays)
        {
            o.SetActive(o == overlay);
        }
    }

    public void HideOverlays()
    {
        foreach (var overlay in overlays)
        {
            overlay.SetActive(false);
        }
        _overlayPanel.SetActive(false); 
    }
}