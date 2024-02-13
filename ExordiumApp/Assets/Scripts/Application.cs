using UnityEngine;

public class Application : MonoBehaviour
{ 
    private void Start()
    {
        InitialFetch();
    }

    private void InitialFetch()
    {
        UIManager.Instance.ShowOverlay(
            UIManager.Instance.PanelMappings[(int)PanelType.Fetching].panelObject
        );

        StartCoroutine(
            ItemService.Instance.FetchItemEntries(itemEntries =>
            {
                DisplayManager.Instance.UpdateItemDisplay(itemEntries);
                ApplicationData.Instance.UpdateItemEntryData(itemEntries);
                UIManager.Instance.HideOverlays();
            })
        );
    }
}