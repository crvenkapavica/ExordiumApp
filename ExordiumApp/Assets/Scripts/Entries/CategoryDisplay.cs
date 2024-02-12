using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryDisplay : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _categry;

    public Toggle Toggle => _toggle;

    public void Setup(ItemCategory categoryEntry)
    {
        _categry.text = categoryEntry.name;
        _toggle.onValueChanged.AddListener((IsOn) => EventManager.Instance.ToggleValueChanged_Category(IsOn, _categry.text));
    }
}
