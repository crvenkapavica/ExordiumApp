using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemService : IItemService
{
    private const string BASE_URL = "https://exordiumgames.com/unity_backend_assignment/";
    private static ItemService s_instance;

    public static ItemService Instance => s_instance ??= new ItemService();

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
        var items = new List<Item>();
        var pageNumber = 1;
        var bMoreItemsAvailable = true;

        while (bMoreItemsAvailable)
        {
            var url = $"{BASE_URL}getitems.php?pageNumber={pageNumber}";
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                if (responseText.Contains("No records found") || string.IsNullOrEmpty(responseText))
                {
                    bMoreItemsAvailable = false;
                }
                else
                {
                    var json = "{\"items\":" + responseText + "}";
                    ItemsResponse response = JsonUtility.FromJson<ItemsResponse>(json);
                    if (response.items != null && response.items.Length > 0)
                    {
                        items.AddRange(response.items);
                        ++pageNumber;
                    }
                    else
                    {
                        bMoreItemsAvailable = false;
                    }
                }
            }
            else
            {
                bMoreItemsAvailable = false; 
            }
        }

        callback?.Invoke(items);
    }
}