using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject _overlayPanel;
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
        _mainPanelAccount.SetActive(true);

        _overlayPanel.SetActive(true);
        _overlayTheme.SetActive(true);
    }
}
