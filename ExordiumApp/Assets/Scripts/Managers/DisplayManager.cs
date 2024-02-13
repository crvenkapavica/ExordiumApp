using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Unity.VisualScripting;

public enum Entry
{
    Item,
    Category,
    Retailer,
    Favorite
}

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager Instance { get; private set; }

    [SerializeField] private Transform[] _contentTransforms;
    [SerializeField] private GameObject[] _entryPrefabs;
    [SerializeField] private ScrollRect[] _scrollRects;

    private readonly List<GameObject> _itemEntries = new();
    private readonly List<GameObject> _categoryEntries = new();
    private readonly List<GameObject> _retailerEntries = new();
    private readonly List<GameObject> _favoriteEntries = new();
    private readonly HashSet<int> _instantiatedFavoritesIds = new();

    private float _height;

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

    public void OnEndDrag()
    {
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
                    ApplicationData.Instance.UpdateItemEntryData(itemEntries);
                    UpdateItemDisplay(itemEntries);
                    ApplyAllItemFilters();
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
            _itemEntries.Add(
                Instantiate(
                    _entryPrefabs[(int)Entry.Item], _contentTransforms[(int)Entry.Item]
                ));

            ThemeManager.Instance.ApplyThemeLocal(_itemEntries[^1].transform);

            if (_height == 0)
            {
                float contentHeight = 
                    _contentTransforms[(int)Entry.Item]
                    .GetComponent<RectTransform>().rect.height;

                _height = (contentHeight - ApplicationData.Instance.Spacing * 5) / 5;
            }

            var rect = _itemEntries[^1].GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _height);

            _itemEntries[^1].GetComponent<ItemDisplay>().Setup(itemEntry);
        }

        _contentTransforms[(int)Entry.Item].GetComponent<ContentSizeFitter>()
            .verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void UpdateCategoryDisplay(List<ItemCategory> categoryEntries)
    {
        foreach(var categoryEntry in categoryEntries)
        {
            _categoryEntries.Add(
                Instantiate(
                    _entryPrefabs[(int)Entry.Category], _contentTransforms[(int)Entry.Category]
                )
            );

            ThemeManager.Instance.ApplyThemeLocal(_categoryEntries[^1].transform);

            _categoryEntries[^1].GetComponent<CategoryDisplay>().Setup(categoryEntry);
        }

        _contentTransforms[(int)Entry.Category].GetComponent<ContentSizeFitter>()
            .verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void UpdateRetailerDisplay(List<Retailer> retailerEntries)
    {
        foreach (var retailerEntry in retailerEntries)
        {
            _retailerEntries.Add( 
                Instantiate(
                    _entryPrefabs[(int)(Entry.Retailer)], _contentTransforms[(int)Entry.Retailer]
                )
            );

            ThemeManager.Instance.ApplyThemeLocal(_retailerEntries[^1].transform);

            _retailerEntries[^1].GetComponent<RetailerDisplay>().Setup(retailerEntry);
        }

        _contentTransforms[(int)Entry.Retailer].GetComponent<ContentSizeFitter>()
            .verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void UpdateFavoritesDisplay()
    {
        foreach (var favorite in UserData.Instance.Favorites)
        {
            foreach (var itemEntry in ApplicationData.Instance.ItemEntries)
            {
                if (itemEntry.Id == favorite && !_instantiatedFavoritesIds.Contains(itemEntry.Id))
                {
                    _favoriteEntries.Add(
                        Instantiate(
                            _entryPrefabs[(int)(Entry.Item)], _contentTransforms[(int)Entry.Favorite]
                        )
                    );

                    _instantiatedFavoritesIds.Add(itemEntry.Id);

                    ThemeManager.Instance.ApplyThemeLocal(_favoriteEntries[^1].transform);

                    _favoriteEntries[^1].GetComponent<ItemDisplay>().Setup(itemEntry, true);
                    
                    break;
                }
            }
        }

        _contentTransforms[(int)Entry.Favorite].GetComponent<ContentSizeFitter>()
            .verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public void ApplyAllItemFilters()
    {
        foreach (var item in _itemEntries)
        {
            var itemDisplay = item.GetComponent<ItemDisplay>();

            if (UserData.Instance.RetailerFilter.Contains(itemDisplay.Retailer)
                || UserData.Instance.CategoryFilter.Contains(itemDisplay.Category))
            {
                item.SetActive(false);
            }
        }
    }
    
    public void AddItemFilter(string filter, bool bIsCategory)
    {
        foreach(var item in _itemEntries)
        {
            if (bIsCategory)
            {
                if (item.GetComponent<ItemDisplay>().Category == filter)
                {
                    item.SetActive(false);
                }
            }
            else
            {
                if (item.GetComponent <ItemDisplay>().Retailer == filter)
                {
                    item.SetActive(false);
                }
            }
        }
    }

    public void RemoveItemFilter()
    {
        foreach (var item in _itemEntries)
        {
            var itemDisplay = item.GetComponent<ItemDisplay>();

            bool retailerActive = !UserData.Instance.RetailerFilter.Contains(itemDisplay.Retailer);
            bool categoryActive = !UserData.Instance.CategoryFilter.Contains(itemDisplay.Category);

            item.SetActive(retailerActive && categoryActive);
        }
    }

    public void RemoveFromFavoritesById(int id)
    {
        GameObject entry = _favoriteEntries.Find(
            entry => entry.GetComponent<ItemDisplay>().Id == id
        );
        _favoriteEntries.Remove(entry);
        Destroy(entry);

        _instantiatedFavoritesIds.Remove(id);

        _itemEntries.Find(entry => entry.GetComponent<ItemDisplay>().Id == id)
            .ConvertTo<ItemDisplay>().FavoritesToggle.isOn = false;
    }

    public IEnumerator LoadImage(string imageUrl, Image targetImage)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            targetImage.sprite = Sprite.Create(
                texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)
            );
        }
    }

    public void ResetValues()
    {
        foreach (var id in _instantiatedFavoritesIds)
        {
            _itemEntries.Find(entry => entry.GetComponent<ItemDisplay>().Id == id)
                .ConvertTo<ItemDisplay>().FavoritesToggle.isOn = false;
        }
        _instantiatedFavoritesIds.Clear();

        foreach (var favorite in _favoriteEntries)
        {
            Destroy(favorite);
        }
        _favoriteEntries.Clear();

        foreach (var item in _itemEntries)
        {
            item.SetActive(true);
        }

        foreach (var retailer in _retailerEntries)
        {
            retailer.GetComponent<RetailerDisplay>().Toggle.isOn = true;
        }

        foreach (var category in _categoryEntries)
        {
            category.GetComponent<CategoryDisplay>().Toggle.isOn = true;
        }
    }

    public void ToggleSavedFavorites()
    {
        foreach (var id in UserData.Instance.Favorites)
        {
            _itemEntries.Find(entry => entry.GetComponent<ItemDisplay>().Id == id)
                .ConvertTo<ItemDisplay>().FavoritesToggle.isOn = true;
        }
    }

    public void ToggleSavedCategories()
    {
        foreach (var category in UserData.Instance.CategoryFilter)
        {
            var component = _categoryEntries.Find(
                entry => entry.GetComponent<CategoryDisplay>().Category == category
            );

            if (component)
                component.ConvertTo<CategoryDisplay>().Toggle.isOn = false;
        }
    }

    public void ToggleSavedRetailers()
    {
        Debug.Log(UserData.Instance.RetailerFilter.Count);

        foreach (var retailer in UserData.Instance.RetailerFilter)
        {
            var component = _retailerEntries.Find(
                entry => entry.GetComponent<RetailerDisplay>().Retailer == retailer
            );

            if (component)
                component.ConvertTo<RetailerDisplay>().Toggle.isOn = false;
        }
    }
}