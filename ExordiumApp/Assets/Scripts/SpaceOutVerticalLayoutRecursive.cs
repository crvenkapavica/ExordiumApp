using UnityEngine;
using UnityEngine.UI;

public class SpaceOutVerticalLayoutRecursive : MonoBehaviour
{
    private void Start()
    {
        SpaceOutLayout(transform);
    }

    private void SpaceOutLayout(Transform parent)
    {
        if (parent.TryGetComponent<VerticalLayoutGroup>(out var verticalLayoutGroup))
        {
            verticalLayoutGroup.spacing = Screen.height * 0.035f;
        }

        foreach (Transform child in parent)
        {
            SpaceOutLayout(child);
        }
    }
}