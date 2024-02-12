using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetailerDisplay : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _retailer;
    [SerializeField] private Image _retailerImage;

    public Toggle Toggle => _toggle;

    public void Setup(Retailer retailerEntry)
    {
        StartCoroutine(
            DisplayManager.Instance.LoadImage(retailerEntry.image_url, _retailerImage)
        );
        _retailer.text = retailerEntry.name;

        _toggle.onValueChanged.AddListener((IsOn) => EventManager.Instance.ToggleValueChanged_Retailer(IsOn, _retailer.text));
    }
}
