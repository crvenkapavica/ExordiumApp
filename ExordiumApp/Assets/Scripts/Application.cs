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
        StartCoroutine(FetchData());
    }

    private IEnumerator FetchData()
    {
        OverlayManager.Instance.ShowOverlay(_overlayFetching);

        yield return StartCoroutine(AttemptFetchRetailerData());
        yield return StartCoroutine(AttemptFetchItemCategoryData());
        yield return StartCoroutine(AttemptFetchItemData());

        OverlayManager.Instance.HideOverlays();
    }

    private IEnumerator AttemptFetchRetailerData()
    {
        yield return StartCoroutine(ItemService.Instance.FetchRetailerData(retailers =>
        {
            ApplicationData.Instance.UpdateRetailerData(retailers);
            Debug.Log(retailers);
        }));
    }

    private IEnumerator AttemptFetchItemCategoryData()
    {
        yield return StartCoroutine(ItemService.Instance.FetchCategoryData(categories =>
        {
            ApplicationData.Instance.UpdateCategoryData(categories);
            Debug.Log(categories);
        }));
    }

    private IEnumerator AttemptFetchItemData()
    {
        yield return StartCoroutine(ItemService.Instance.FetchItemData(items =>
        {
            ApplicationData.Instance.UpdateItemData(items);
            Debug.Log(items);
        }));
    }
}
