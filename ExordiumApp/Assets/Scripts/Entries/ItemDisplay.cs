using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _category;
    [SerializeField] private Image _retailerImage;
    [SerializeField] private Toggle _favoritesToggle;

    public Toggle FavoritesToggle => _favoritesToggle;

    public int Id { get; private set; }

    public void Setup(ItemEntry itemEntry, bool bIsOn = false)
    {
        StartCoroutine(
            DisplayManager.Instance.LoadImage(itemEntry.ItemImageUrl, _itemImage)
        );
        StartCoroutine(
            DisplayManager.Instance.LoadImage(itemEntry.RetailerImageUrl, _retailerImage)
        );

        Id = itemEntry.Id;

        _price.text = itemEntry.Price.ToString("C", CultureInfo.CreateSpecificCulture("nl-NL"));
        _itemName.text = itemEntry.ItemName;
        _category.text = itemEntry.CategoryName;

        _favoritesToggle.isOn = bIsOn;
        _favoritesToggle.onValueChanged.AddListener(
            (IsOn) => EventManager.Instance.ToggleValueChanged_Favorite(Id, IsOn)
        );
    }

    public void IsOn(bool bIsOn)
    {
        _favoritesToggle.isOn = bIsOn;
    }
}