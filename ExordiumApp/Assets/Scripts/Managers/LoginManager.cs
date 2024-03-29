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
        if (!UserService.Instance.ValidateUserInput(username, password)) return;

        StartCoroutine(
            UserService.Instance.Login(username, password, (success, message) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_Welcome
                    );

                    DisplayManager.Instance.ResetValues();

                    UserData.Instance.UpdateLoginStatus(true, username);
                    UIManager.Instance.ToggleAccountPanel(false, username);

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
        UserService.Instance.Logout();
    }
}