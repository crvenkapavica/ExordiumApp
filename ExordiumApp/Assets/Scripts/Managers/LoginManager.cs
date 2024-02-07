using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public void AttemptLogin(string username, string password)
    {
        StartCoroutine(UserService.Instance.Login(username, password, (success, message) =>
        {
            if (success)
            {
                Debug.Log("Login successful: " + message);
            }
            else
            {
                Debug.LogError("Login failed: " + message);
            }
        }));
    }
}