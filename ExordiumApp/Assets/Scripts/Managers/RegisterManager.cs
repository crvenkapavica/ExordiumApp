using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    public static RegisterManager Instance { get; private set; }

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

    public void AttemptRegister(string username, string password)
    {
        if (!UserService.Instance.ValidateUserInput(username, password)) return;

        StartCoroutine(
            UserService.Instance.Register(username, password, (success, message) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_OK
                    );
                }
                else
                {
                    UIManager.Instance.ShowOverlay(
                        UIManager.Instance.PanelMappings[(int)PanelType.MessageBox].panelObject, EMessageBoxResponse.Response_Email
                    );
                }
            })
        );
    }
}