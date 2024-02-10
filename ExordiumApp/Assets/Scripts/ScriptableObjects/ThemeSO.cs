using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color PanelBackgroundColor;
    public Color TextColor;
    public Color ButtonBackgroundColor;
    public string ThemeName;
}