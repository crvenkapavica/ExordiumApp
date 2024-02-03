public class UserData
{
    public static UserData Instance { get; private set; } = new UserData();
    public string Username { get; private set; }
    public bool IsLoggedIn { get; private set; }

    public void UpdateLoginStatus(string username, bool isLoggedIn)
    {
        Username = username;
        IsLoggedIn = isLoggedIn;
    }
}