using System;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public string description;
    public string image_url;
    public decimal price;
    public int retailer_id;
    public int item_category_id;
}

[Serializable]
public class ItemsResponse
{
    public Item[] items;
}

[Serializable]
public class Retailer
{
    public int id;
    public string name;
    public string image_url;
}

[Serializable]
public class RetailersResponse
{
    public Retailer[] retailers;
}

[Serializable]
public class ItemCategory
{
    public int id;
    public string name;
}

[Serializable]
public class ItemCategoriesResponse
{
    public ItemCategory[] itemCategories;
}

[Serializable]
public class RegisterLoginResponse
{
    public bool isSuccessful;
    public string message;
}
