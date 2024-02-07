using System.Collections.Generic;

public class ApplicationData
{
    public static ApplicationData Instance { get; private set; } = new();

    public List<Item> Items { get; private set; } = new();
    public List<Retailer> Retailers { get; private set; } = new();
    public List<ItemCategory> ItemCategories { get; private set; } = new();

    public void UpdateItems(List<Item> items)
    {
        Items = items;
    }

    public void UpdateRetailers(List<Retailer> retailers)
    {
        Retailers = retailers;
    }

    public void UpdateItemCategories(List<ItemCategory> itemCategories)
    {
        ItemCategories = itemCategories;
    }
}
