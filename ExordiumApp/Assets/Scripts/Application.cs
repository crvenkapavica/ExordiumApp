using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour
{ 
    private void Start()
    {
        ApplicationData.Instance.LoadDefaultPrefs();
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
                UIManager.Instance.HideOverlays();
            })
        );
    }
}