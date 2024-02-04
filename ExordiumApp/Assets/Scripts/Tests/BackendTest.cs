using UnityEngine;

public class BackendTest : MonoBehaviour
{
    private readonly string _username = "ExUser1101";
    private readonly string _password = "ExPw1101";

    private void Start()
    {
        //AttemptRegister(_username, _password);

        AttemptLogin(_username, _password);

        //AttemptGetItems(2);
        //AttemptGetRetailers();
        //AttemptGetItemCategories();
    }

    private void AttemptRegister(string username, string password)
    {
        StartCoroutine(UserService.Instance.Register(username, password, (success, message) =>
        {
            if (success)
            {
                Debug.Log("Successful registration: " + message);
                AttemptLogin(username, password);
            }
            else
            {
                Debug.Log("Registration failed: " + message);
            }
        }));
    }

    private void AttemptLogin(string username, string password)
    {
        StartCoroutine(UserService.Instance.Login(username, password, (success, message) =>
        {
            if (success)
            {
                Debug.Log("Successful Log In: " + message);
                UserData.Instance.UpdateLoginStatus(username, true);
            }
        }));
    }

    private void AttemptGetItems(int pageNumber)
    {
        StartCoroutine(ItemService.Instance.GetItems(pageNumber, items =>
        {
            Debug.Log($"Fetched {items.Count} items.");
            ApplicationData.Instance.UpdateItems(items);
        }));
    }

    private void AttemptGetRetailers()
    {
        StartCoroutine(ItemService.Instance.GetRetailers(retailers =>
        {
            Debug.Log($"Fetched {retailers.Count} retailers");
            ApplicationData.Instance.UpdateRetailers(retailers);
        }));
    }

    private void AttemptGetItemCategories()
    {
        StartCoroutine(ItemService.Instance.GetItemCategories(itemCategories =>
        {
            Debug.Log($"Fetched {itemCategories.Count} itemCategories");
            ApplicationData.Instance.UpdateItemCategories(itemCategories);
        }));
    }
}