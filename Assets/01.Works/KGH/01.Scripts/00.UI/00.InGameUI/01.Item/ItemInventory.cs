using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInventory
{
    private TabElement _itemTab;
    private VisualElement _tabContent;
    private VisualTreeAsset _itemListTemplate;
    private InventoryManager _inventoryManager;
    private Dictionary<VisualElement, ItemTab> _itemTabs;
    private Dictionary<string, VisualElement> _itemTabElements;

    private InGameUI _inGameUI;
    
    public ItemInventory(TabElement itemTab,VisualTreeAsset itemListTemplate, InventoryManager inventoryManager, InGameUI inGameUI)
    {
        _itemTab = itemTab;
        _inventoryManager = inventoryManager;
        _itemListTemplate = itemListTemplate;
        
        _itemTabs = new Dictionary<VisualElement, ItemTab>();   
        _itemTabElements = new Dictionary<string, VisualElement>();
        
        inventoryManager.OnInventoryChanged += HandleInventoryChanged;
        inventoryManager.OnInventoryInitialized += HandleInventoryInitialized;

        _tabContent = _itemTab.Q<VisualElement>("TabContents");
        _inGameUI = inGameUI;
    }

    private void HandleInventoryInitialized(List<DefaultItemInventory> defaultItemInventories)
    {
        _itemTab.TabNames = "";
        foreach (var defaultItemInventory in defaultItemInventories)
        {
            AddItemTab(defaultItemInventory);
        }

        _itemTab.TabCount = defaultItemInventories.Count;
    }

    private void HandleInventoryChanged(string location)
    {
        var updatedTab = _itemTabElements[location];
        _itemTabs[updatedTab].UpdateItemTab();
    }
    public void AddItemTab(InventoryParents parentItem)
    {
        if(_itemTabElements.ContainsKey(parentItem.name)) return;
        
        _itemTab.TabNames += $"{parentItem.name},";
        _itemTab.TabCount++;

        var tab = _itemListTemplate.CloneTree();
        tab.name = parentItem.name;
        
        _tabContent.Add(tab);
        _itemTabElements.Add(parentItem.name, tab);
        _itemTabs.Add(tab, new ItemTab(tab, parentItem, this, _inventoryManager.ToolNames, _inGameUI));
        
        _itemTabs[tab].UpdateItemTab();
    }
}
