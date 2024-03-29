using System.Collections.Generic;

public class ItemEntry
{
    public int Id { get; set; }

    public string ItemName { get; set; }

    public float Price { get; set; }

    public string ItemImageUrl { get; set; }

    public string RetailerImageUrl { get; set; }

    public string RetailerName { get; set; }

    public string CategoryName { get; set; }
}

public class ApplicationData
{
    public static ApplicationData Instance { get; private set; } = new();

    public float Spacing { get; set; }

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

    public void UpdateItemEntryData(List<ItemEntry> itemEntries)
    {
        ItemEntries.AddRange(itemEntries);
    }

    public void SetDefaultPrefs()
    {
        ThemeManager.Instance.Theme = ThemeManager.Instance.DarkTheme;
        LocalizationManager.Instance.Language = Language.Croatian;
        DisplayManager.Instance.ResetValues();
        UserData.Instance.Favorites.Clear();
        UserData.Instance.RetailerFilter.Clear();
        UserData.Instance.CategoryFilter.Clear();
    }
}
