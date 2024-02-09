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
        StartCoroutine(
            UserService.Instance.Register(username, password, (success, message) =>
            {
                if (success)
                {
                    Debug.Log("Registration successful: " + message);
                }
                else
                {
                    Debug.Log("Registration failed: " + message);
                }
            })
        );
    }
}