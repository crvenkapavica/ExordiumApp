using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserService : IUserService
{
    private const string BASE_URL = "https://exordiumgames.com/unity_backend_assignment/";
    private static UserService s_instance;

    public static UserService Instance => s_instance ??= new UserService();

    public IEnumerator Register(string username, string password, Action<bool, string> callback)
    {
        Debug.Log("LOGGING FROM REGISTER: " + username + " " + password);
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest request = UnityWebRequest.Post(BASE_URL + "register.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<RegisterLoginResponse>(request.downloadHandler.text);
            callback(response.isSuccessful, response.message);
        }
        else
        {
            callback(false, request.error);
        }
    }

    public IEnumerator Login(string username, string password, Action<bool, string> callback)
    {
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest request = UnityWebRequest.Post(BASE_URL + "login.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<RegisterLoginResponse>(request.downloadHandler.text);
            callback(response.isSuccessful, response.message);
        }
        else
        {
            callback(false, request.error);
        }
    }
}
