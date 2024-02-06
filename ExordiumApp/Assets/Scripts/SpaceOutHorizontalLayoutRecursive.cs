using UnityEngine;
using UnityEngine.UI;

public class SpaceOutHorizontalLayoutRecursive : MonoBehaviour
{
    private void Start()
    {
        SpaceOutLayout(transform);
    }

    private void SpaceOutLayout(Transform parent)
    {
        if (parent.TryGetComponent<HorizontalLayoutGroup>(out var horizontalLayoutGroup))
        {
            horizontalLayoutGroup.spacing = Screen.height * 0.035f;
        }

        foreach (Transform child in parent)
        {
            SpaceOutLayout(child);
        }
    }
}
