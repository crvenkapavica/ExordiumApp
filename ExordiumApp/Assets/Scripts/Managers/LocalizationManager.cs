using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Language
{
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

        Language = (_language = Language.Croatian);
    }

    public void LocalizeTextRecursive(Transform parent, Language language)
    {
        if (parent.TryGetComponent(out TextMeshProUGUI text)
            && _translationDictionary.TryGetValue(text.name, out TranslationItem translation))
        {
            if (!string.IsNullOrEmpty(text.name))
            {
                text.text = language == Language.English ? translation.english : translation.croatian;
            }
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            LocalizeTextRecursive(parent.GetChild(i), language);
        }
    }

    public void ApplyLocalization(Language language, bool bPermanent)
    {
        Language = bPermanent
            ? _language = (language == Language.None ? _language : _previewLanguge)
            : _previewLanguge = language;

        if (language == Language.None)
        {
            UIManager.Instance.LanguageToggles[(int)Language].isOn = true;
        }
    }
}
