using System;
using System.Collections.Generic;

// back-end JSON
////////////////////////////////////////////
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
public class CredentialResponse
{
    public bool isSuccessful;
    public string message;
}
////////////////////////////////////////////

// Language JSON
////////////////////////////////////////////
[Serializable]
public class TranslationItems   
{
    public List<TranslationItem> translations;
}

[Serializable]
public class TranslationItem
{
    public string name;
    public string english;
    public string croatian;
}
////////////////////////////////////////////


// User Services
////////////////////////////////////////////
[Serializable]
public class UserCredentials
{
    public string username;
    public string password;
}
////////////////////////////////////////////
