using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Collections;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _category;
    [SerializeField] private Image _retailerImage;
    [SerializeField] private Toggle _favoritesToggle;

    public Toggle FavoritesToggle => _favoritesToggle;

    public string Category => _category.text;

    public string Retailer { get; private set; }

    public int Id { get; private set; }

    private string _itemImageUrl;
    private string _retailerImageUrl;

    private bool _bItemImageLoaded;
    private bool _bRetailerImageLoaded;

    public bool IsFinishedLoading => _bItemImageLoaded && _bRetailerImageLoaded;

    public void Setup(ItemEntry itemEntry, bool bIsOn = false)
    {
        _itemImageUrl = itemEntry.ItemImageUrl; 
        _retailerImageUrl = itemEntry.RetailerImageUrl;

        Id = itemEntry.Id;
        Retailer = itemEntry.RetailerName;

        _price.text = itemEntry.Price.ToString("C", CultureInfo.CreateSpecificCulture("nl-NL"));
        _itemName.text = itemEntry.ItemName;
        _category.text = _category.name = itemEntry.CategoryName;

        _favoritesToggle.isOn = bIsOn;
        _favoritesToggle.onValueChanged.AddListener(
            (IsOn) => EventManager.Instance.ToggleValueChanged_Favorite(Id, IsOn)
        );
    }

    private void OnEnable()
    {
        if (!IsFinishedLoading)
        {
            StartCoroutine(LoadImages());
        }
    }

    private IEnumerator LoadImages()
    {
        yield return new WaitUntil(() => 
            !string.IsNullOrEmpty(_itemImageUrl) && !string.IsNullOrEmpty(_retailerImageUrl)
        );

        if (!_bItemImageLoaded)
        {
            StartCoroutine(
                DisplayManager.Instance.LoadImage(
                    _itemImageUrl, _itemImage, (() => _bItemImageLoaded = true)
                )
            );
        }

        if (!_bRetailerImageLoaded)
        {
            StartCoroutine(
                DisplayManager.Instance.LoadImage(
                    _retailerImageUrl, _retailerImage, (() => _bRetailerImageLoaded = true)
                )
            );
        }
    }
}