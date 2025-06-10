using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private TabUI baseTabUI;
    [SerializeField] private WorldRecipeCalculator baseTabObject;
    
    [Header("UI")]
    [SerializeField] private Transform tabContainer;
    [SerializeField] private TabUI tabUIPrefab;

    [Header("Calculator")]
    [SerializeField] private Transform tabObjectContainer;
    [SerializeField] private WorldRecipeCalculator tabObjectPrefab;

    private Dictionary<TabUI, WorldRecipeCalculator> tabs = new Dictionary<TabUI, WorldRecipeCalculator>();
    private KeyValuePair<TabUI, WorldRecipeCalculator> selectedTab;

    private void Awake()
    {
        tabs.Add(baseTabUI, baseTabObject);
    }

    private void Start()
    {
        OpenTab(baseTabUI);
    }

    private void OpenTab(TabUI tab)
    {
        if (selectedTab.Key == tab)
            return;
        
        if (selectedTab.Key != tab && selectedTab.Key != null)
        {
            selectedTab.Key.HighlightTab(false);
            selectedTab.Value.gameObject.SetActive(false);
        }

        selectedTab = new KeyValuePair<TabUI,WorldRecipeCalculator>(tab, tabs[tab]);
        tab.HighlightTab(true);
        tabs[tab].gameObject.SetActive(true);
    }

    private void CloseTab(TabUI tab)
    {
        if (!tabs.ContainsKey(tab))
            return;
        int index = tab.transform.GetSiblingIndex() - 1;
        Destroy(tabs[tab]);
        tabs.Remove(tab);
        Destroy(tab.gameObject);
        
        OpenTab(tabs.ElementAt(index).Key);
    }

    public void ChangeTabUIInfo(WorldRecipeCalculator calculator, Part part)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs.ElementAt(i).Value == calculator)
            {
                tabs.ElementAt(i).Key.text.text = part.name;
                tabs.ElementAt(i).Key.icon.sprite = part.icon;
            }
        }
    }

    public void OnAddTab()
    {
        TabUI tabUI = Instantiate(tabUIPrefab, tabContainer);
        tabUI.transform.SetSiblingIndex(tabContainer.childCount - 2);
        tabUI.manager = this;

        WorldRecipeCalculator tabObject = Instantiate(tabObjectPrefab, tabObjectContainer);
        tabObject.tabManager = this;
        tabs.Add(tabUI, tabObject);
        
        OpenTab(tabUI);
    }

    public void OnOpenTab(TabUI tab)
    {
        OpenTab(tab);
    }
    
    public void OnCloseTab(TabUI tab)
    {
        CloseTab(tab);
    }
}
