using System;
using System.Collections;
using System.Collections.Generic;

public interface IUserService
{
    IEnumerator Register(string username, string password, Action<bool, string> callback);
    IEnumerator Login(string username, string password, Action<bool, string> callback);
}

public interface IItemService
{
    IEnumerator FetchRetailerData(Action<List<Retailer>> callback);
    IEnumerator FetchCategoryData(Action<List<ItemCategory>> callback);
    IEnumerator FetchItemData(Action<List<Item>> callback);
}