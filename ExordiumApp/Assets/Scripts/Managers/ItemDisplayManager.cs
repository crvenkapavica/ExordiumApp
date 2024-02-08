using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemDisplayManager : MonoBehaviour, IEndDragHandler
{
    public static ItemDisplayManager Instance { get; private set; }

    [SerializeField] private Transform _itemsParent;
    [SerializeField] private GameObject _itemEntryPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    private bool _bIsFetching = false;
    private bool _bCanFetchMore = true;

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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the scroll view is at the bottom
        if (_scrollRect.verticalNormalizedPosition <= 0.05f && !_bIsFetching && _bCanFetchMore)
        {
            FetchMoreItems();
        }
    }

    private void FetchMoreItems()
    {
        if (!_bCanFetchMore) return;

        _bIsFetching = true;
        StartCoroutine(
            ItemService.Instance.FetchItemData(items =>
            {
                if (items.Count == 0)
                {
                    _bCanFetchMore = false;
                }
                else
                {
                    ApplicationData.Instance.UpdateItemData(items);
                    UpdateItemDisplay(items);
                }
                _bIsFetching = false;
            })
        );
    }

    public void UpdateItemDisplay(List<Item> items)
    {
        foreach (var item in items)
        {
            GameObject itemObject = Instantiate(_itemEntryPrefab, _itemsParent);

            float height = _itemsParent.GetComponent<RectTransform>().rect.height / 5;
            var rect = itemObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

            itemObject.GetComponent<ItemDisplay>().Setup(item);
        }
    }
}