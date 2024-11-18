using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftInventory
{
    private TabElement _craftTab;
    private VisualTreeAsset _craftListTemplate;
    private InventoryManager _inventoryManager;
    
    private Dictionary<ToolType, CraftTab> _craftTabs;
    
    public CraftInventory(TabElement craftTab, VisualTreeAsset craftListTemplate, InventoryManager inventoryManager)
    {
        _craftTab = craftTab;
        _craftListTemplate = craftListTemplate;
        _inventoryManager = inventoryManager;

        foreach (var type in Enum.GetValues(typeof(ToolType)))
        {
        }
    }
}
