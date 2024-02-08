using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _category;
    [SerializeField] private Image _retailerImage;
    [SerializeField] private Toggle _favoritesToggle;
    
    // Assuming your Item class has these properties
    public void Setup(Item item)
    {
        // Load image from URL async
        // _itemImage.sprite = 
        _itemImage.color = Color.green;

        _price.text = item.price.ToString();
        _itemName.text = item.name;
        _category.text = item.item_category_id.ToString();

        _retailerImage.color = Color.red;
        //_favoritesToggle = 
    }
}