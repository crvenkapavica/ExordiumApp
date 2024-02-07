using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour
{
    // Main paneles
    [SerializeField] private GameObject _mainPanelBasePrefab;
    [SerializeField] private GameObject _mainPanelAccountPrefab;
    [SerializeField] private GameObject _mainPanelSettingsPrefab;
    [SerializeField] private GameObject _navigationPanelPrefab;

    [SerializeField] private GameObject _titleBarPrefab;

    // Overlays
    [SerializeField] private GameObject _overlayLanguagePrefab;
    [SerializeField] private GameObject _overlayThemePrefab;
    [SerializeField] private GameObject _messageBoxPrefab;
    [SerializeField] private GameObject _overlayFetchingPrefab;

    // Entries
    [SerializeField] private GameObject _itemEntryPrefab;
    [SerializeField] private Toggle _categoryTogglePrefab;
    [SerializeField] private Toggle _retailerTogglePrefab;
    [SerializeField] private Toggle _languageTogglePrefab;



    private void Start()
    {
        if (_overlayFetchingPrefab.TryGetComponent(out RectTransform overlay))
        {
            overlay.sizeDelta = new Vector2(overlay.sizeDelta.x, Screen.height * 0.4f);
        }

        Instantiate(_overlayFetchingPrefab, transform);
    }
}
