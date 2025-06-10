using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    public TabManager manager;
    
    public TMP_Text text;
    public Image icon;
    [SerializeField] private Image highlight;

    public void HighlightTab(bool value)
    {
        highlight.gameObject.SetActive(value);
    }
    
    public void OnCloseTab()
    {
        manager.OnCloseTab(this);
    }

    public void OnSelectTab()
    {
        manager.OnOpenTab(this);
    }
}
