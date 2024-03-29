using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class UserService : IUserService
{
    private const string BASE_URL = "https://exordiumgames.com/unity_backend_assignment/";

    private static UserService s_instance;

    public static UserService Instance => s_instance ??= new UserService();

    public IEnumerator Register(string username, string password, Action<bool, string> callback)
    {
        var userCredentials = new UserCredentials
        {
            username = username,
            password = password
        };

        var form = new WWWForm();
        form.AddField("json", JsonConvert.SerializeObject(userCredentials));

        using UnityWebRequest request = UnityWebRequest.Post(BASE_URL + "register.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<CredentialResponse>(request.downloadHandler.text);
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
        var userCredentials = new UserCredentials
        {
            username = username,
            password = password
        };
        form.AddField("json", JsonConvert.SerializeObject(userCredentials));

        using UnityWebRequest request = UnityWebRequest.Post(BASE_URL + "login.php", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<CredentialResponse>(request.downloadHandler.text);
            callback(response.isSuccessful, response.message);
        }
        else
        {
            callback(false, request.error);
        }
    }

    public bool ValidateUserInput(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
            || !username.Contains("@") || !username.Contains(".")
            || username.Contains("@.") || username.Contains(".@")
            || username[0] == '.' || username[0] == '@'
            || username[^1] == '@' || username[^1] == '.')
        {
            UIManager.Instance.ShowOverlay(
                UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_Input
            );
            return false;
        }

        return true;
    }

    public void Logout()
    {
        UserData.Instance.UpdateLoginStatus(false, string.Empty);
        UIManager.Instance.ToggleAccountPanel(true);
        ApplicationData.Instance.SetDefaultPrefs();
    }
}
