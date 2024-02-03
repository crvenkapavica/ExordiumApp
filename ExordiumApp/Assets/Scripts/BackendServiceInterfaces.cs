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
    IEnumerator GetItems(int pageNumber, Action<List<Item>> callback);
    IEnumerator GetRetailers(Action<List<Retailer>> callback);
    IEnumerator GetItemCategories(Action<List<ItemCategory>> callback);
}