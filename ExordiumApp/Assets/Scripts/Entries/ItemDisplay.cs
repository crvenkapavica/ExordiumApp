using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Globalization;

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
        StartCoroutine(LoadImage(itemEntry.ItemImageUrl, _itemImage));
        StartCoroutine(LoadImage(itemEntry.RetailerImageUrl, _retailerImage));

        _price.text = itemEntry.Price.ToString("C", new CultureInfo("eu-EU"));
        _itemName.text = itemEntry.ItemName;
        _category.text = itemEntry.CategoryName;

        //_favoritesToggle = 
    }

    private IEnumerator LoadImage(string imageUrl, Image image)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}