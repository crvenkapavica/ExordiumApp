using System;
using System.Collections;
using System.Collections.Generic;

public interface IUserService
{
    /// <summary>
    /// Registers the user using RegisterManager.
    /// </summary>
    /// <param name="username">User's e-mail.</param>
    /// <param name="password">User's password.</param>
    /// <param name="callback">Saves the user data into UserData and shows overlays using UIManager.</param>
    /// <returns></returns>
    IEnumerator Register(string username, string password, Action<bool, string> callback);

    /// <summary>
    /// Checks for credentials and logs the user in using LoginManager.
    /// </summary>
    /// <param name="username">User's e-mail.</param>
    /// <param name="password">User's password.</param>
    /// <param name="callback">Saves the user data into UserData and shows overlays using UIManager.</param>
    /// <returns></returns>
    IEnumerator Login(string username, string password, Action<bool, string> callback);

    /// <summary>
    /// Log out of current session, save user settings, and reset ApplicationData to default.
    /// </summary>
    void Logout();
}

public interface IItemService
{
    /// <summary>
    /// Fetch Retailer data at application start-up.
    /// </summary>
    /// <param name="callback">Updates ApplicationData and displays the data using DisplayManager.</param>
    /// <returns></returns>
    IEnumerator FetchRetailerData(Action<List<Retailer>> callback);

    /// <summary>
    /// Fetch Category data at application start-up.
    /// </summary>
    /// <param name="callback">Updates ApplicationData and displays the data using DisplayManager.</param>
    /// <returns></returns>
    IEnumerator FetchCategoryData(Action<List<ItemCategory>> callback);

    /// <summary>
    /// Simulates fetching batches of 8 at a time since the back-end doesn't
    /// provide the parametrization for fetch size.
    /// </summary>
    /// <param name="callback">Updates ApplicationData and displays the data using DisplayManager.</param>
    /// <returns></returns>
    IEnumerator FetchItemData(Action<List<Item>> callback);
}