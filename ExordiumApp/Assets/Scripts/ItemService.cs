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

    public IEnumerator GetItems(int pageNumber, Action<List<Item>> callback)
    {
        var url = $"{BASE_URL}getitems.php?pageNumber={pageNumber}";
        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var json = "{\"items\":" + request.downloadHandler.text + "}";
            var response = JsonUtility.FromJson<ItemsResponse>(json);
            if (response.items != null)
            {
                callback?.Invoke(new List<Item>(response.items));
            }
            else
            {
                Debug.LogError("Failed to fetch items: Response contained no items.");
                callback?.Invoke(new List<Item>());
            }
        }
        else
        {
            Debug.LogError($"Failed to fetch items: {request.error}");
            callback?.Invoke(new List<Item>());
        }
    }

    public IEnumerator GetRetailers(Action<List<Retailer>> callback)
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
                Debug.LogError("Failed to fetch retailers: Response contained no retailers.");
                callback?.Invoke(new List<Retailer>());
            }
        }
        else
        {
            Debug.LogError($"Failed to fetch retailers: {request.error}");
            callback?.Invoke(new List<Retailer>());
        }
    }

    public IEnumerator GetItemCategories(Action<List<ItemCategory>> callback)
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
                Debug.LogError("Failed to fetch item categories: Response contained no item categories.");
                callback?.Invoke(new List<ItemCategory>());
            }
        }
        else
        {
            Debug.LogError($"Failed to fetch item categories: {request.error}");
            callback?.Invoke(new List<ItemCategory>());
        }
    }
}