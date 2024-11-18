using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftInventory
{
    private InventoryManager _inventoryManager;

    private Dictionary<ToolType, CraftTab> _craftTabs = new Dictionary<ToolType, CraftTab>();

    public CraftInventory(TabElement craftTab, VisualTreeAsset craftListTemplate, InventoryManager inventoryManager)
    {
        var craftItems = inventoryManager.ItemListSO.ItemList;
        var craftTabContainer = craftTab.Q<VisualElement>("TabContents");
        var typeName = inventoryManager.ToolNames;
        _inventoryManager = inventoryManager;

        craftTab.TabNames = "";
        var enumCount = Enum.GetValues(typeof(ToolType)).Length;
        for (var i = 0; i < enumCount; i++)
        {
            var tab = craftListTemplate.CloneTree();
            var items = craftItems.FindAll(x => (x.itemType == ItemType.Tool) && (x.toolType == (ToolType)i));
            var craft = new CraftTab(items, tab.Q<ScrollView>(), inventoryManager);
            _craftTabs.Add((ToolType)i, craft);
            craftTab.TabNames += typeName[(ToolType)i] + ", ";
            craftTabContainer.Add(tab);
        }

        craftTab.TabCount = enumCount;
    }
}