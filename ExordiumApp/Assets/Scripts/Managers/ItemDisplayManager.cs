using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemDisplayManager : MonoBehaviour, IEndDragHandler
{
    public static ItemDisplayManager Instance { get; private set; }

    private float _height;

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
        if (!_bCanFetchMore || _bIsFetching) return;

        _bIsFetching = true;
        StartCoroutine(
            ItemService.Instance.FetchItemEntries(itemEntries =>
            {
                if (itemEntries.Count == 0)
                {
                    _bCanFetchMore = false;          
                }
                else
                {
                    UpdateItemDisplay(itemEntries);
                }
                _bIsFetching = false;
            })
        );
    }

    public void UpdateItemDisplay(List<ItemEntry> itemEntries)
    {
        foreach (var itemEntry in itemEntries)
        {
            GameObject itemObject = Instantiate(_itemEntryPrefab, _itemsParent);

            if (_height == 0)
            {
                float contentHeight = _itemsParent.GetComponent<RectTransform>().rect.height;
                _height = (contentHeight - Screen.height * 0.035f * 5) / 5;
            }
            var rect = itemObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _height);

            itemObject.GetComponent<ItemDisplay>().Setup(itemEntry);
        }

        _itemsParent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}