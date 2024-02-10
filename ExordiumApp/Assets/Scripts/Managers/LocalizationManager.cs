using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public enum Language
{
    Null = -2,
    None = -1,
    Croatian,
    English,
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private readonly Dictionary<string, TranslationItem> _translationDictionary = new();

    private Language _language = Language.None;
    private Language _previewLanguge = Language.None;

    public Language Language
    {
        get => _language;
        set
        {
            _previewLanguge = value;
            LocalizeTextRecursive(transform, _previewLanguge);
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

        Language = Language.Croatian;
    }

    public void LocalizeTextRecursive(Transform parent, Language language)
    {
        if (parent.TryGetComponent(out TextMeshProUGUI text)
            && _translationDictionary.TryGetValue(text.name, out TranslationItem translation))
        {
            text.text = language == Language.English ? translation.english : translation.croatian;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            LocalizeTextRecursive(parent.GetChild(i), language);
        }
    }

    public void ApplyLocalization(Language language, bool bPermanent)
    {
        Language = bPermanent
            ? _language = (language == Language.Null ? _language : _previewLanguge)
            : _previewLanguge = language;
    }
}
