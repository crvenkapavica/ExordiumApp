using System.Collections.Generic;
using System.Linq;

public class ItemEntry
{
    public string ItemName { get; set; }
    public float Price { get; set; }
    public string ItemImageUrl { get; set; }
    public string RetailerImageUrl { get; set; }
    public string CategoryName { get; set; }
}

public class ApplicationData
{
    public static ApplicationData Instance { get; private set; } = new();

    public List<Item> Items { get; private set; } = new();
    public List<Retailer> Retailers { get; private set; } = new();
    public List<ItemCategory> Categories { get; private set; } = new();
    public List<ItemEntry> ItemEntries { get; private set; } = new();

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

    public void GenerateItemEntries()
    {
        ItemEntries = 
            (from item in Items
             join retailer in Retailers on item.retailer_id equals retailer.id
             join category in Categories on item.item_category_id equals category.id
             select new ItemEntry
             {

                 ItemName = item.name,
                 Price = item.price,
                 ItemImageUrl = item.image_url,
                 RetailerImageUrl = retailer.image_url,
                 CategoryName = category.name
             }).ToList();
    }
}
