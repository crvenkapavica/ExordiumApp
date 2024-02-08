using UnityEngine;
using UnityEngine.UI;

public class SpaceOutLayoutRecursive : MonoBehaviour
{
    private float _spacing = Screen.height * 0.035f;

    private void Start()
    {
        ApplicationData.Instance.Spacing = _spacing;

        SpaceOutLayoutHorizontal(transform);
        SpaceOutLayoutVertical(transform);
    }

    private void SpaceOutLayoutHorizontal(Transform parent)
    {
        if (parent.TryGetComponent<HorizontalLayoutGroup>(out var horizontalLayoutGroup))
        {
            horizontalLayoutGroup.spacing = _spacing;
        }

        foreach (Transform child in parent)
        {
            SpaceOutLayoutHorizontal(child);
        }
    }

    private void SpaceOutLayoutVertical(Transform parent)
    {
        if (parent.TryGetComponent<VerticalLayoutGroup>(out var verticalLayoutGroup))
        {
            verticalLayoutGroup.spacing = _spacing;
        }

        foreach (Transform child in parent)
        {
            SpaceOutLayoutVertical(child);
        }
    }
}
