using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    public void AttemptRegister(string username, string password)
    {
        StartCoroutine(UserService.Instance.Register(username, password, (success, message) =>
        {
            if (success)
            {
                Debug.Log("Registration successful: " + message);
            }
            else
            {
                Debug.LogError("Registration failed: " + message);
            }
        }));
    }
}