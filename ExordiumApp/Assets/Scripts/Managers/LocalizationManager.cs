using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private readonly Dictionary<string, TranslationItem> _translationDictionary = new();

    private string _language;

    public string Language 
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                LocalizeTextRecursive(transform);
            }
        } 
    }

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

    private void Start()
    {
        string json = Resources.Load<TextAsset>("localization").text;
        TranslationItems translationItems = JsonUtility.FromJson<TranslationItems>(json);

        foreach (var item in translationItems.translations)
        {
            _translationDictionary[item.name] = item;
        }

        Language = "croatian";
    }

    public void LocalizeTextRecursive(Transform parent)
    {
        if (parent.TryGetComponent(out TextMeshProUGUI text)
            && _translationDictionary.TryGetValue(text.name, out TranslationItem translation))
        {
            text.text = Language == "english" ? translation.english : translation.croatian;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            LocalizeTextRecursive(parent.GetChild(i));
        }
    }
}
