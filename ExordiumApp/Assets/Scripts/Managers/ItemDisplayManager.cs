using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public enum Entry
{
    Item,
    Category,
    Retailer,
    Favorite
}

public class ItemDisplayManager : MonoBehaviour, IEndDragHandler
{
    public static ItemDisplayManager Instance { get; private set; }

    private float _height;

    [SerializeField] private Transform[] _contentTransforms;
    [SerializeField] private GameObject[] _entryPrefabs;
    [SerializeField] private ScrollRect[] _scrollRects;

    private bool _bIsFetching = false;
    private bool _bCanFetchMore = true;

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

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the scroll view is at the bottom
        if (_scrollRects[(int)Entry.Item].verticalNormalizedPosition <= 0.05f && !_bIsFetching && _bCanFetchMore)
        {
            FetchMoreItems();
        }
    }

    private void FetchMoreItems()
    {
        if (!_bCanFetchMore || _bIsFetching) return;

        _bIsFetching = true;
        StartCoroutine(
            ItemService.Instance.FetchItemEntries(itemEntries =>
            {
                if (itemEntries.Count > 0)
                {
                    UpdateItemDisplay(itemEntries);
                }
                else
                {
                    _bCanFetchMore = false;
                }
                _bIsFetching = false;
            })
        );
    }

    public void UpdateItemDisplay(List<ItemEntry> itemEntries)
    {
        foreach (var itemEntry in itemEntries)
        {
            GameObject itemObject = 
                Instantiate(
                    _entryPrefabs[(int)Entry.Item], _contentTransforms[(int)Entry.Item]
                );

            ThemeManager.Instance.ApplyThemeLocal(itemObject.transform);

            if (_height == 0)
            {
                float contentHeight = _contentTransforms[(int)Entry.Item]
                    .GetComponent<RectTransform>().rect.height;

                _height = (contentHeight - ApplicationData.Instance.Spacing * 5) / 5;
            }

            var rect = itemObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _height);

            itemObject.GetComponent<ItemDisplay>().Setup(itemEntry);
        }

        _contentTransforms[(int)Entry.Item].GetComponent<ContentSizeFitter>()
            .verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void UpdateCategoryDisplay(List<ItemCategory> categoryEntries)
    {
        foreach(var categoryEntry in categoryEntries)
        {
            GameObject categoryObject =
                Instantiate(
                    _entryPrefabs[(int)Entry.Category], _contentTransforms[(int)Entry.Category]
                );

            ThemeManager.Instance.ApplyThemeLocal(categoryObject.transform);

            categoryObject.GetComponent<CategoryDisplay>().Setup(categoryEntry);
        }
    }

    public void UpdateRetailerDisplay(List<Retailer> retailerEntries)
    {
        foreach (var retailerEntry in retailerEntries)
        {
            GameObject retailerObject = 
                Instantiate(
                    _entryPrefabs[(int)(Entry.Retailer)], _contentTransforms[(int)Entry.Retailer]
                );

            ThemeManager.Instance.ApplyThemeLocal(retailerObject.transform);

            retailerObject.GetComponent<RetailerDisplay>().Setup(retailerEntry);
        }
    }

    public IEnumerator LoadImage(string imageUrl, Image targetImage)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}