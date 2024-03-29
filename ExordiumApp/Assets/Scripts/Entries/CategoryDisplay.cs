using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryDisplay : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _categry;

    public Toggle Toggle => _toggle;
    public string Category => _categry.text;

    public void Setup(ItemCategory categoryEntry)
    {
        _categry.text = _categry.name = categoryEntry.name;

        _toggle.onValueChanged.AddListener(
            (IsOn) => EventManager.Instance.ToggleValueChanged_Category(IsOn, _categry.text)
        );
    }
}
