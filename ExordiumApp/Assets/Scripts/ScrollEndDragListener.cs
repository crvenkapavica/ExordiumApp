using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollEndDragListener : MonoBehaviour, IEndDragHandler
{
    public void OnEndDrag(PointerEventData eventData)
    {
        DisplayManager.Instance.OnEndDrag();
    }
}