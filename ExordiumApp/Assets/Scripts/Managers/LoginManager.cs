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
                    Debug.Log("Login successful: " + message);
                }
                else
                {
                    Debug.Log("Login failed: " + message);
                }
            })
        );
    }
}