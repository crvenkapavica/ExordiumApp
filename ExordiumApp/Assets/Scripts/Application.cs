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

    // Overlays
    [SerializeField] private GameObject _overlayLanguage;
    [SerializeField] private GameObject _overlayTheme;
    [SerializeField] private GameObject _messageBox;

    // Entries
    [SerializeField] private GameObject _itemEntry;
    [SerializeField] private Toggle _categoryToggle;
    [SerializeField] private Toggle _retailerToggle;
    [SerializeField] private Toggle _languageToggle;

    /// REST OF SERIALIZED FIELDS
    /// 
}
