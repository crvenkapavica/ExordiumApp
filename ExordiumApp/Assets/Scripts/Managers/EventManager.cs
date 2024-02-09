using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

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

        AssignButtonEvents();
    }

    private void AssignButtonEvents()
    {
        foreach (var panelMapping in UIManager.Instance.PanelMappings)
        {
            GameObject panel = panelMapping.panelObject;
            panel.SetActive(true);

            Button[] buttons = panel.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                switch (button.name)
                {
                    case "Items":
                        button.onClick.AddListener(() => ButtonClicked_Items());            break;
                    case "Category":
                        button.onClick.AddListener(() => ButtonClicked_Category());         break;
                    case "Account":
                        button.onClick.AddListener(() => ButtonClicked_Account());          break;
                    case "Retailer":
                        button.onClick.AddListener(() => ButtonClicked_Retailer());         break;
                    case "Favorites":
                        button.onClick.AddListener(() => ButtonClicked_Favorites());        break;
                    case "Settings":
                        button.onClick.AddListener(() => ButtonClicked_Settings());         break;
                    case "Register":
                        button.onClick.AddListener(() => ButtonClicked_Register(button));   break;
                    case "Login":
                        button.onClick.AddListener(() => ButtonClicked_Login(button));      break;

                        
                    default:
                        break;
                }
            }

            if (panelMapping.panelType != PanelType.Navigation && panelMapping.panelType != PanelType.Items)
            {
                panel.SetActive(false);
            }
        }
    }


    // NAVIGATION
    private void ButtonClicked_Items()
    {
        UIManager.Instance.ShowPanel(PanelType.Items);
    }

    private void ButtonClicked_Category()
    {
        UIManager.Instance.ShowPanel(PanelType.Category);
    }

    private void ButtonClicked_Account()
    {
        UIManager.Instance.ShowPanel(PanelType.Account);
    }

    private void ButtonClicked_Retailer()
    {
        UIManager.Instance.ShowPanel(PanelType.Retailer);
    }

    private void ButtonClicked_Favorites()
    {
        UIManager.Instance.ShowPanel(PanelType.Favorites);
    }

    private void ButtonClicked_Settings()
    {
        UIManager.Instance.ShowPanel(PanelType.Settings);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////
    
    // ACCOUNT
    private void ButtonClicked_Register(Button button)
    {
        Transform parent = button.transform.parent.parent.parent;
        TextMeshProUGUI emailInput = parent.Find("EmailPanel/Input/TextArea/EmailInput").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI passwordInput = parent.Find("PasswordPanel/Input/TextArea/PasswordInput").GetComponent<TextMeshProUGUI>();

        RegisterManager.Instance.AttemptRegister(emailInput.text, passwordInput.text);

        parent.Find("EmailPanel/Input").GetComponent<TMP_InputField>().text = "";
        parent.Find("PasswordPanel/Input").GetComponent<TMP_InputField>().text = "";
    }
    private void ButtonClicked_Login(Button button)
    {
        Transform parent = button.transform.parent.parent.parent;
        TextMeshProUGUI emailInput = parent.Find("EmailPanel/Input/TextArea/EmailInput").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI passwordInput = parent.Find("PasswordPanel/Input/TextArea/PasswordInput").GetComponent<TextMeshProUGUI>();

        LoginManager.Instance.AttemptLogin(emailInput.text, passwordInput.text);

        parent.Find("EmailPanel/Input").GetComponent<TMP_InputField>().text = "";
        parent.Find("PasswordPanel/Input").GetComponent<TMP_InputField>().text = "";
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////
}










//public class EventManager : MonoBehaviour
//{
//    public static EventManager Instance { get; private set; }

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // Optional: Keep alive for the lifetime of the game
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//        AssignButtonListeners();
//    }

//    private void AssignButtonListeners()
//    {
//        Button[] buttons = FindObjectsOfType<Button>();
//        foreach (var button in buttons)
//        {
//            button.onClick.AddListener(() => ButtonClicked(button));
//        }
//    }

//    private void ButtonClicked(Button button)
//    {
//        switch (button.name)
//        {
//            case "NavButton1":
//                ShowPanel(PanelType.Panel1);
//                break;
//            default:
//                Debug.Log("Clicked on " + button.name);
//                break;
//        }
//    }

//    private void ShowPanel(PanelType panelType)
//    {
//    }
//}

//// Enum to identify panels - adjust based on your actual panels
//public enum PanelType
//{
//    Panel1,
//    Panel2,
//}
