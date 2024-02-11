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
        StartCoroutine(
            UserService.Instance.Login(username, password, (success, message) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_Welcome
                    );

                    UIManager.Instance.ToggleAccountPanel(false, username);
                    UserData.Instance.UpdateLoginStatus(true, username);
                    UserData.Instance.LoadPlayerPrefs();
                }
                else
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_Credentials
                    );

                    UserData.Instance.Username = string.Empty;
                }
            })
        );
    }

    public void Logout()
    {
        UserData.Instance.SavePlayerPrefs();
        UIManager.Instance.ToggleAccountPanel(true);
        UserData.Instance.UpdateLoginStatus(false, string.Empty);
        ApplicationData.Instance.LoadDefaultPrefs();
    }
}