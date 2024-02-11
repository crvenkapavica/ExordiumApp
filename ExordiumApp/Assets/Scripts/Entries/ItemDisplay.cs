using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _category;
    [SerializeField] private Image _retailerImage;
    [SerializeField] private Toggle _favoritesToggle;
    

    public void Setup(ItemEntry itemEntry)
    {
        StartCoroutine(
            DisplayManager.Instance.LoadImage(itemEntry.ItemImageUrl, _itemImage)
        );
        StartCoroutine(
            DisplayManager.Instance.LoadImage(itemEntry.RetailerImageUrl, _retailerImage)
        );

        _price.text = itemEntry.Price.ToString("C");
        _itemName.text = itemEntry.ItemName;
        _category.text = itemEntry.CategoryName;

        //_favoritesToggle = OnClickEvent.
    }
}