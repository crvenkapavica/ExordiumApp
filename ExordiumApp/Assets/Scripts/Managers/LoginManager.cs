using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AttemptLogin(string username, string password)
    {
        string _username = username;

        StartCoroutine(
            UserService.Instance.Login(username, password, (success, message) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, UIManager.EMessageBoxResponse.Response_Welcome
                    );

                    UIManager.Instance.ToggleAccountPanel(false, _username);
                    UserData.Instance.UpdateLoginStatus(true, _username);
                }
                else
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, UIManager.EMessageBoxResponse.Response_Credentials
                    );
                }
            })
        );
    }

    public void Logout()
    {
        UIManager.Instance.ToggleAccountPanel(true);
        UserData.Instance.UpdateLoginStatus(false);
    }
}