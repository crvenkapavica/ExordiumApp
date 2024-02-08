using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ItemService : MonoBehaviour, IItemService
{
    public static ItemService Instance { get; private set; }

    private const string BASE_URL = "https://exordiumgames.com/unity_backend_assignment/";

    private const int FETCH_COUNT = 8;

    private int _pageNumber = 1;

    private int _fetchedItemsOnPage;

    private bool _bAllItemsFetched;

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

    public IEnumerator FetchRetailerData(Action<List<Retailer>> callback)
    {
        var url = BASE_URL + "getretailers.php";
        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var json = "{\"retailers\":" + request.downloadHandler.text + "}";
            var response = JsonUtility.FromJson<RetailersResponse>(json);
            if (response.retailers != null)
            {
                callback?.Invoke(new List<Retailer>(response.retailers));
            }
            else
            {
                callback?.Invoke(new List<Retailer>());
            }
        }
        else
        {
            callback?.Invoke(new List<Retailer>());
        }
    }

    public IEnumerator FetchCategoryData(Action<List<ItemCategory>> callback)
    {
        var url = BASE_URL + "getitemcategories.php";
        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var json = "{\"itemCategories\":" + request.downloadHandler.text + "}";
            var response = JsonUtility.FromJson<ItemCategoriesResponse>(json);
            if (response.itemCategories != null)
            {
                callback?.Invoke(new List<ItemCategory>(response.itemCategories));
            }
            else
            {
                callback?.Invoke(new List<ItemCategory>());
            }
        }
        else
        {
            callback?.Invoke(new List<ItemCategory>());
        }
    }

    public IEnumerator FetchItemData(Action<List<Item>> callback)
    {
        if (_bAllItemsFetched) yield break;

        var currentItemsFetched = new List<Item>();
        var bIsFetchingComplete = false;

        while (!bIsFetchingComplete)
        {
            var url = $"{BASE_URL}getitems.php?pageNumber={_pageNumber}";
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = "{\"items\":" + request.downloadHandler.text + "}";
                ItemsResponse response = JsonUtility.FromJson<ItemsResponse>(json);

                if (response.items != null && response.items.Length > 0)
                {
                    int itemsToFetch = response.items.Length - _fetchedItemsOnPage;
                    for (int i = 0; i < itemsToFetch && currentItemsFetched.Count < FETCH_COUNT; i++)
                    {
                        currentItemsFetched.Add(response.items[_fetchedItemsOnPage + i]);
                    }

                    // Add the current fetched items to total count
                    _fetchedItemsOnPage += currentItemsFetched.Count;

                    if (currentItemsFetched.Count < FETCH_COUNT)
                    {
                        if (itemsToFetch < FETCH_COUNT)
                        {
                            // If we fetched fewer items than expected, move to the next page
                            _pageNumber++;
                            _fetchedItemsOnPage = 0; // Reset counter since we're moving to a new page
                        }
                        // If itemsToFetch is 8 or more but currentItemsFetched is still less than 8,
                        // it means we've reached the end of available items on this page.
                    }
                    else
                    {
                        // We've fetched enough items.
                        bIsFetchingComplete = true; 
                    }
                }
                else
                {
                    // No more items to fetch
                    _bAllItemsFetched = true;
                    bIsFetchingComplete = true;
                }
            }
            else
            {
                // Request failed, stop fetching
                _bAllItemsFetched = true;
                bIsFetchingComplete = true;
            }
        }

        callback?.Invoke(currentItemsFetched);
    }

    public IEnumerator FetchItemEntries(Action<List<ItemEntry>> callback)
    {
        if (ApplicationData.Instance.Retailers.Count == 0 || ApplicationData.Instance.Categories.Count == 0)
        {
            yield return FetchRetailerData(retailers => ApplicationData.Instance.UpdateRetailerData(retailers));
            yield return FetchCategoryData(categories => ApplicationData.Instance.UpdateCategoryData(categories));
        }

        if (ApplicationData.Instance.Items.Count == 0 || _fetchedItemsOnPage >= FETCH_COUNT)
        {
            yield return FetchItemData(items =>
            {
                ApplicationData.Instance.UpdateItemData(items);
                _fetchedItemsOnPage = 0;
                _pageNumber++; 
            });
        }

        var itemEntries = (from item in ApplicationData.Instance.Items
                           join retailer in ApplicationData.Instance.Retailers on item.retailer_id equals retailer.id
                           join category in ApplicationData.Instance.Categories on item.item_category_id equals category.id
                           select new ItemEntry
                           {
                               ItemName = item.name,
                               Price = item.price,
                               ItemImageUrl = item.image_url,
                               RetailerImageUrl = retailer.image_url,
                               CategoryName = category.name
                           }).ToList();

        callback?.Invoke(itemEntries);
    }
}













//public IEnumerator FetchData(Action onComplete = null)
//{
//    yield return StartCoroutine(AttemptFetchRetailerData());
//    yield return StartCoroutine(AttemptFetchItemCategoryData());
//    yield return StartCoroutine(AttemptFetchItemData());

//    onComplete?.Invoke();
//}

//private IEnumerator AttemptFetchRetailerData()
//{
//    yield return StartCoroutine(
//        FetchRetailerData(retailers =>
//        {
//            ApplicationData.Instance.UpdateRetailerData(retailers);
//        })
//    );
//}

//private IEnumerator AttemptFetchItemCategoryData()
//{
//    yield return StartCoroutine(
//        FetchCategoryData(categories =>
//        {
//            ApplicationData.Instance.UpdateCategoryData(categories);
//        })
//    );
//}

//private IEnumerator AttemptFetchItemData()
//{
//    yield return StartCoroutine(
//        FetchItemData(items =>
//        {
//            ApplicationData.Instance.UpdateItemData(items);

//            // Join the needed details  
//            ApplicationData.Instance.GenerateItemEntries();

//            // Call UI logic to add the newly fetched 8 items to ScrollView
//            //ItemDisplayManager.Instance.UpdateItemDisplay(items);
//        })
//    );
//}