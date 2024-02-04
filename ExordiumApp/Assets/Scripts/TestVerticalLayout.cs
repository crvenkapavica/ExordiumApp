using UnityEngine;
using UnityEngine.UI;

public class AdjustVerticalLayoutSpacing : MonoBehaviour
{
    private void Start()
    {
        AdjustSpacingRecursive(transform);
    }

    private void AdjustSpacingRecursive(Transform parent)
    {
        var layoutGroup = parent.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.spacing = Screen.height * 0.035f;
        }

        foreach (Transform child in parent)
        {
            AdjustSpacingRecursive(child);
        }
    }
}