using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryDisplay : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _categry;

    public void Setup(ItemCategory categoryEntry)
    {
        _categry.text = categoryEntry.name;
        //_toggle.onValueChanged = ..
    }
}
