using UnityEngine;
using UnityEngine.UI;

public class SpaceOutHorizontalLayout : MonoBehaviour
{
    private void Start()
    {
        SpaceOutLayout();
    }

    private void SpaceOutLayout()
    {
        GetComponent<HorizontalLayoutGroup>().spacing = Screen.height * 0.035f;
    }
}
