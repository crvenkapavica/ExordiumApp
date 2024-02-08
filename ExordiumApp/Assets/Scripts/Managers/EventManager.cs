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

        SetButtonEvents();
    }

    public void AddEvent(GameObject go)
    {

    }

    private void SetButtonEvents()
    {
        Button[] buttons = FindObjectsOfType<Button>();
    }
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
