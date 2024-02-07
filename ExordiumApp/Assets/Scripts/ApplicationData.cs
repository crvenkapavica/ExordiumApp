using System.Collections.Generic;

public class ApplicationData
{
    public static ApplicationData Instance { get; private set; } = new();

    public List<Item> Items { get; private set; } = new();
    public List<Retailer> Retailers { get; private set; } = new();
    public List<ItemCategory> Categories { get; private set; } = new();

    public void UpdateItemData(List<Item> items)
    {
        Items = items;
    }

    public void UpdateRetailerData(List<Retailer> retailers)
    {
        Retailers = retailers;
    }

    public void UpdateCategoryData(List<ItemCategory> categories)
    {
        Categories = categories;
    }
}
