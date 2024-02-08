using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour
{
    // Main paneles
    [SerializeField] private GameObject _mainPanelBase;
    [SerializeField] private GameObject _mainPanelAccount;
    [SerializeField] private GameObject _mainPanelSettings;
    [SerializeField] private GameObject _navigationPanel;

    // Overlays
    [SerializeField] private GameObject _overlayLanguage;
    [SerializeField] private GameObject _overlayTheme;
    [SerializeField] private GameObject _overlayMessageBox;
    [SerializeField] private GameObject _overlayFetching;

    // Entries
    [SerializeField] private GameObject _itemEntryPrefab;
    [SerializeField] private Toggle _categoryTogglePrefab;
    [SerializeField] private Toggle _retailerTogglePrefab;
    [SerializeField] private Toggle _languageTogglePrefab;
    
    private void Start()
    {
        InitialFetch();

        StartCoroutine(TestOverlay());
    }

    private IEnumerator TestOverlay()
    {
        yield return new WaitForSeconds(2);

        OverlayManager.Instance.ShowOverlay(_overlayMessageBox);
        yield return new WaitForSeconds(5);
        OverlayManager.Instance.HideOverlays();

        yield return new WaitForSeconds(1);

        OverlayManager.Instance.ShowOverlay(_overlayTheme);
        yield return new WaitForSeconds(5);
        OverlayManager.Instance.HideOverlays();

        yield return new WaitForSeconds(1);

        OverlayManager.Instance.ShowOverlay(_overlayLanguage);
        yield return new WaitForSeconds(5);
        OverlayManager.Instance.HideOverlays();

        yield return new WaitForSeconds(1);

        OverlayManager.Instance.ShowOverlay(_overlayFetching);
        yield return new WaitForSeconds(5);
        OverlayManager.Instance.HideOverlays();

    }

    private void InitialFetch()
    {
        OverlayManager.Instance.ShowOverlay(_overlayFetching);
        StartCoroutine(
            ItemService.Instance.FetchItemEntries(itemEntries =>
            {
                ItemDisplayManager.Instance.UpdateItemDisplay(itemEntries);
                OverlayManager.Instance.HideOverlays();
            })
        );
    }
}